using Blanner.Data;
using Blanner.Data.Models;
using Blanner.Hubs;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace Blanner.Api;
public static class GoalsEndpoints {
	public static void MapGoals(this IEndpointRouteBuilder endpoints) {
		ArgumentNullException.ThrowIfNull(endpoints);

		var goalsGroup = endpoints.MapGroup("/goals");

		goalsGroup.MapGet("/", GoalsEndpointsBehaviors.Goals);
		goalsGroup.MapGet("/{goalId}", GoalsEndpointsBehaviors.Goal);
		goalsGroup.MapPost("/start", GoalsEndpointsBehaviors.StartTimer);
		goalsGroup.MapPost("/activate", GoalsEndpointsBehaviors.PushToActive);
		goalsGroup.MapPost("/save", GoalsEndpointsBehaviors.CreateGoal);
		goalsGroup.MapPost("/delete", GoalsEndpointsBehaviors.DeleteGoal);
		goalsGroup.MapPost("/save/header", GoalsEndpointsBehaviors.SaveHeaderChanges);

		var activeGoalsGroup = goalsGroup.MapGroup("/active");

		activeGoalsGroup.MapGet("/", ActiveGoalsEndpointsBehaviors.Goals);
		activeGoalsGroup.MapGet("/{goalId}", ActiveGoalsEndpointsBehaviors.Goal);
		activeGoalsGroup.MapPost("/start", ActiveGoalsEndpointsBehaviors.StartTimer);
		activeGoalsGroup.MapPost("/stop", ActiveGoalsEndpointsBehaviors.StopTimer);
		activeGoalsGroup.MapPost("/create", ActiveGoalsEndpointsBehaviors.CreateGoal);
		activeGoalsGroup.MapPost("/persist", ActiveGoalsEndpointsBehaviors.PersistGoal);
		activeGoalsGroup.MapPost("/delete", ActiveGoalsEndpointsBehaviors.DeleteGoal);
		activeGoalsGroup.MapPost("/complete", ActiveGoalsEndpointsBehaviors.CompleteJob);
		activeGoalsGroup.MapPost("/save/header", ActiveGoalsEndpointsBehaviors.SaveHeaderChanges);
		activeGoalsGroup.MapPost("/time/add", ActiveGoalsEndpointsBehaviors.AddTimer);
		activeGoalsGroup.MapPost("/time/edit", ActiveGoalsEndpointsBehaviors.EditTimer);
		activeGoalsGroup.MapPost("/time/delete", ActiveGoalsEndpointsBehaviors.DeleteTimer);
	}
}

public static class GoalsEndpointsBehaviors {
	public static async Task<IResult> Goals(
		[FromQuery] string userId,
		[FromQuery] bool includeActive,
		[FromServices] GoalsRepository goalsRepository)
	{
		var goals = (await goalsRepository.Goals(userId, includeActive).ToListAsync()).Select(x => new GoalData(x)).ToList();

		return TypedResults.Json(goals);
	}

	public static async Task<IResult> Goal(
		[FromRoute] int goalId,
		[FromServices] GoalsRepository goalsRepository)
	{
		var goal = await goalsRepository.Goal(goalId);

		return goal is null ? TypedResults.NotFound() : TypedResults.Json(new GoalDetailsData(goal));
	}

	public static async Task<IResult> CreateGoal(
		[FromBody] GoalCreationData request,
		[FromServices] GoalsRepository goalsRepository,
		[FromServices] IHubContext<GoalsHub, IGoalsClient> hubContext) 
	{
		Goal? goal = await goalsRepository.Save(request.UserId, request.Name);
		if (goal is null) return TypedResults.BadRequest();
		await hubContext.Clients.All.GoalCreated(goal.Id, request.UserId, new GoalData(goal));
		return TypedResults.Ok();
	}

	public static async Task<IResult> DeleteGoal(
		[FromBody] GoalDeleteData request,
		[FromServices] GoalsRepository goalsRepository,
		[FromServices] IHubContext<GoalsHub, IGoalsClient> hubContext) 
	{
		Goal? goal = await goalsRepository.Delete(request.GoalId);
		if (goal is null) return TypedResults.NotFound();
		await hubContext.Clients.All.GoalDeleted(request.GoalId, request.UserId);
		return TypedResults.Ok();
	}

	public static async Task<IResult> StartTimer(
		[FromBody] TimerActivationData request,
		[FromServices] GoalsRepository goalsRepository,
		[FromServices] ActiveGoalsRepository activeGoalsRepository,
		[FromServices] IHubContext<GoalsHub, IGoalsClient> goalsHubContext) 
	{
		if (!request.GoalId.HasValue) return TypedResults.BadRequest();

		ActiveGoal goal = await goalsRepository.Activate(request.GoalId.Value, request.ActivationDate);
		_ = await activeGoalsRepository.StartTimer(goal, request.ActivationDate);

		await goalsHubContext.Clients.All.GoalActivated(goal.Goal?.Id ?? 0, goal.Id, request.UserId, new(goal));

		return TypedResults.Ok();
	}

	public static async Task<IResult> PushToActive(
		[FromBody] GoalPushToActiveData request,
		[FromServices] GoalsRepository goalsRepository,
		[FromServices] IHubContext<GoalsHub, IGoalsClient> hubContext) 
	{
		ActiveGoal activeGoal = await goalsRepository.Activate(request.GoalId, request.ActivationDate);
		await hubContext.Clients.All.GoalActivated(request.GoalId, activeGoal.Id, request.UserId, new(activeGoal));
		return TypedResults.Ok();
	}
	public static async Task<IResult> SaveHeaderChanges(
		[FromBody] GoalHeaderChangesSaveData request,
		[FromServices] GoalsRepository goalsRepository,
		[FromServices] IHubContext<GoalsHub, IGoalsClient> hubContext) 
	{
		Goal goal = await goalsRepository.Update(request.GoalId, request.Name, request.ContractorId);
		GoalHeaderData data = new(goal.Name, goal.Contractor);
		await hubContext.Clients.All.GoalHeaderEdited(request.GoalId, request.UserId, data);
		return TypedResults.Ok();
	}
}

public static class ActiveGoalsEndpointsBehaviors {
	public static async Task<IResult> Goals(
		[FromQuery] string userId,
		[FromServices] ActiveGoalsRepository goalsRepository)
	{
		var goals = (await goalsRepository.Goals(userId).Include(x => x.Goal).ToListAsync()).Select(x => new ActiveGoalData(x)).ToList();

		return TypedResults.Json(goals);
	}

	public static async Task<IResult> Goal(
		[FromRoute] int goalId,
		[FromServices] ActiveGoalsRepository goalsRepository)
	{
		var goal = await goalsRepository.Goal(goalId);

		return goal is null ? TypedResults.NotFound() : TypedResults.Json(new ActiveGoalDetailsData(goal));
	}

	public static async Task<IResult> CreateGoal(
		[FromBody] GoalCreationData request,
		[FromServices] ActiveGoalsRepository goalsRepository,
		[FromServices] IHubContext<GoalsHub, IGoalsClient> hubContext) {
		ActiveGoal goal = await goalsRepository.Create(request);
		await hubContext.Clients.All.ActiveGoalCreated(goal.Id, request.UserId, new ActiveGoalData(goal));
		return TypedResults.Ok();
	}

	public static async Task<IResult> PersistGoal(
		[FromBody] ActiveGoalPersistData request,
		[FromServices] ActiveGoalsRepository repository,
		[FromServices] IHubContext<GoalsHub, IGoalsClient> hubContext) {
		Goal? goal = await repository.Persist(request);
		if (goal is null) return TypedResults.BadRequest();
		await hubContext.Clients.All.GoalCreated(goal.Id, request.UserId, new GoalData(goal));
		return TypedResults.Ok();
	}


	public static async Task<IResult> DeleteGoal(
		[FromBody] GoalDeleteData request,
		[FromServices] ActiveGoalsRepository goalsRepository,
		[FromServices] IHubContext<GoalsHub, IGoalsClient> hubContext) {
		ActiveGoal? goal = await goalsRepository.Delete(request.GoalId);
		if (goal is null) return TypedResults.NotFound();
		await hubContext.Clients.All.ActiveGoalDeleted(request.GoalId, request.UserId);
		return TypedResults.Ok();
	}

	public static async Task<IResult> StartTimer(
		[FromBody] TimerActivationData request,
		[FromServices] ActiveGoalsRepository activeGoalsRepository,
		[FromServices] IHubContext<GoalsHub, IGoalsClient> goalsHubContext) 
	{
		if (!request.ActiveGoalId.HasValue) return TypedResults.BadRequest();

		ActiveGoal goal = await activeGoalsRepository.StartTimer(request.ActiveGoalId.Value, request.ActivationDate);

		await goalsHubContext.Clients.All.GoalActivated(goal.Goal?.Id ?? 0, goal.Id, request.UserId, new(goal));

		return TypedResults.Ok();
	}

	public static async Task<IResult> StopTimer(
		[FromBody] TimerDeactivationData request,
		[FromServices] ActiveGoalsRepository activeGoalsRepository,
		[FromServices] IHubContext<GoalsHub, IGoalsClient> goalsHubContext) 
	{
		var goalTotalTime = await activeGoalsRepository.StopTimer(request.GoalId, request.StopDate);

		await goalsHubContext.Clients.All.TimerStopped(request.GoalId, request.UserId, goalTotalTime);

		return TypedResults.Ok();
	}

	public static async Task<IResult> SaveHeaderChanges(
		[FromBody] ActiveGoalHeaderChangesSaveData request,
		[FromServices] ActiveGoalsRepository goalsRepository,
		[FromServices] IHubContext<GoalsHub, IGoalsClient> hubContext)
	{
		ActiveGoal goal = await goalsRepository.Update(request);

		ActiveGoalHeaderData data = new(goal.Name, goal.Comment, goal.Contractor);
		await hubContext.Clients.All.ActiveGoalHeaderEdited(request.GoalId, request.UserId, data);

		return TypedResults.Ok();
	}

	public static async Task<IResult> AddTimer(
		[FromBody] GoalTimeCreationData request,
		[FromServices] ActiveGoalsRepository goalsRepository,
		[FromServices] IHubContext<GoalsHub, IGoalsClient> hubContext) {
		(ActiveGoalTime GoalTime, TimeSpan GoalTotalTime)? result = await goalsRepository.CreateTimer(request);
		if (result is null) return TypedResults.Problem();
		await hubContext.Clients.All.ActiveGoalTimerCreated(request.GoalId, request.UserId, new(result.Value.GoalTime), result.Value.GoalTotalTime);
		return TypedResults.Ok();
	}
	
	public static async Task<IResult> EditTimer(
		[FromBody] GoalTimeEditData request,
		[FromServices] ActiveGoalsRepository goalsRepository,
		[FromServices] IHubContext<GoalsHub, IGoalsClient> hubContext) {
		(ActiveGoalTime GoalTime, TimeSpan GoalTotalTime)? result = await goalsRepository.EditTimer(request);
		if (result is null) return TypedResults.Problem();
		await hubContext.Clients.All.ActiveGoalTimerEdited(request.GoalId, request.UserId, new(result.Value.GoalTime), result.Value.GoalTotalTime);
		return TypedResults.Ok();
	}

	public static async Task<IResult> DeleteTimer(
		[FromBody] GoalTimeDeleteData request,
		[FromServices] ActiveGoalsRepository goalsRepository,
		[FromServices] IHubContext<GoalsHub, IGoalsClient> hubContext) {
		ActiveGoalTime? goalTIme = await goalsRepository.DeleteTimer(request.TimeId);
		if (goalTIme is null) return TypedResults.NotFound();
		await hubContext.Clients.All.ActiveGoalTimerDeleted(request.GoalId, request.TimeId, request.UserId, await goalsRepository.TotalTime(request.GoalId));
		return TypedResults.Ok();
	}


	public static async Task<IResult> CompleteJob(
		[FromBody] CompleteJobData request,
		[FromServices] ActiveGoalsRepository repository,
		[FromServices] IHubContext<GoalsHub, IGoalsClient> hubContext) {
		List<ActiveGoal> goals = await repository.CompleteJob(request.UserId, request.CompleteDate);
		
		await Parallel.ForEachAsync(goals, async (goal, token) =>
		{
			await hubContext.Clients.All.TimerStopped(goal.Id, request.UserId, goal.TotalTime());
		});
		return TypedResults.Ok();
	}
}