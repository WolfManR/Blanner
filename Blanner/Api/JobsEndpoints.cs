using Blanner.Data.Models;
using Blanner.Data;
using Microsoft.AspNetCore.Mvc;
using Blanner.Hubs;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace Blanner.Api;

public static class JobsEndpoints {
	public static void MapJobs(this IEndpointRouteBuilder endpoints) {
		var jobsGroup = endpoints.MapGroup("/jobs");

		jobsGroup.MapPost("/", JobsEndpointsBehaviors.Jobs);
		jobsGroup.MapGet("/{jobId}", JobsEndpointsBehaviors.Job);
		jobsGroup.MapPost("/build", JobsEndpointsBehaviors.BuildJob);
	}
}

public static class JobsEndpointsBehaviors {
	public static async Task<IResult> Jobs([FromBody] JobsListData request,[FromServices] JobsRepository repository) {
		var data = await repository.List(request.UserId, request.Start, request.End);
		return TypedResults.Json(data);
	}
	
	public static async Task<IResult> Job([FromRoute] int jobId, [FromServices] JobsRepository repository) {
		var data = await repository.Details(jobId);
		return TypedResults.Json(data);
	}


	public static async Task<IResult> BuildJob(
		[FromBody] BuildJobData request,
		[FromServices] ApplicationDbContext dbContext,
		[FromServices] IHubContext<GoalsHub, IGoalsClient> hubContext) {
		User? user = await dbContext.Users.FindAsync(request.UserId);
		if (user is null) return TypedResults.BadRequest();

		await using (var transaction = await dbContext.Database.BeginTransactionAsync()) {
			var activeGoals = await dbContext.ActiveGoals.AsNoTracking()
			.Include(x => x.User)
			.Where(x => x.User != null && x.User.Id == request.UserId && x.CurrentlyActiveTime != null)
			.Select(x => new { GoalId = x.Id, TimerId = x.CurrentlyActiveTime!.Value })
			.ToDictionaryAsync(x => x.GoalId, x => x.TimerId);

			var activeTimersId = activeGoals.Values.ToHashSet();
			var activeGoalsId = activeGoals.Keys.ToHashSet();

			await dbContext.ActiveGoalsTime.Where(x => activeTimersId.Contains(x.Id)).ExecuteUpdateAsync(setters => setters.SetProperty(x => x.End, request.BuildDate));
			await dbContext.ActiveGoals.Where(x => activeGoalsId.Contains(x.Id)).ExecuteUpdateAsync(setters => setters.SetProperty(x => x.CurrentlyActiveTime, x => null));

			await transaction.CommitAsync();
		}

		var goals = await dbContext.ActiveGoals.AsNoTracking()
			.Include(x => x.User).Where(x => x.User != null && x.User.Id == request.UserId)
			.Include(x => x.Contractor)
			.Include(x => x.Tasks.Where(x => x.Done))
			.Include(x => x.GoalTime)
			.ToListAsync();
		
		await using (var transaction = await dbContext.Database.BeginTransactionAsync()) {
			await dbContext.ActiveGoals
				.Include(x => x.User)
				.Include(x => x.GoalTime)
				.Where(x => x.User != null && x.User.Id == request.UserId)
				.ExecuteDeleteAsync();

				await dbContext.ToDos
					.Include(x => x.User).Where(x => x.User != null && x.User.Id == request.UserId)
					.Include(x => x.Goal)
					.Include(x => x.ActiveGoal)
					.Where(x => x.Done && x.Goal != null && x.ActiveGoal != null)
					.ExecuteDeleteAsync();
			
			await transaction.CommitAsync();
		}

		Dictionary<int, Contractor> attachedContractors = [];

		foreach (var goal in goals)
		{
			Contractor? attachedContractor = null;
			if(goal.Contractor is { Id: { } contractorId }) {
				if(!attachedContractors.TryGetValue(contractorId, out attachedContractor)) {
					attachedContractor = await dbContext.Contractors.FindAsync(contractorId);
					if(attachedContractor is not null) {
						attachedContractors.TryAdd(contractorId , attachedContractor);
					}
				}
			}

			JobContext context = new() {
				Contractor = attachedContractor,
				Comment = goal.Comment,
				Name = goal.Name
			};

			dbContext.Jobs.Update(context);

			foreach (var time in goal.GoalTime)
			{
				JobTime jobTime = new() {
					User = user,
					Context = context,
					Start = time.Start,
					End = time.End
				};

				dbContext.JobsTime.Update(jobTime);
			}
		}

		await dbContext.SaveChangesAsync();

		await hubContext.Clients.All.JobsBuilded(request.UserId);
		return TypedResults.Ok();
	}
}

