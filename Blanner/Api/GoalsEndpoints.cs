using Blanner.Data;
using Blanner.Data.Models.TimeRanges;
using Blanner.Hubs;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace Blanner.Api;
public static class GoalsEndpoints {
	public static void MapGoals(this IEndpointRouteBuilder endpoints) {
		ArgumentNullException.ThrowIfNull(endpoints);

		var goalsGroup = endpoints.MapGroup("/goals");

		var goalsTemplatesGroup = goalsGroup.MapGroup("/templates");

		goalsTemplatesGroup.MapGet("/", GoalsTemplatesEndpointsBehaviors.Goals);
		goalsTemplatesGroup.MapGet("/{goalId}", GoalsTemplatesEndpointsBehaviors.Goal);
		goalsTemplatesGroup.MapPost("/activate", GoalsTemplatesEndpointsBehaviors.ActivateGoalByTemplate);
		goalsTemplatesGroup.MapPost("/save", GoalsTemplatesEndpointsBehaviors.CreateGoal);
		goalsTemplatesGroup.MapPost("/delete", GoalsTemplatesEndpointsBehaviors.DeleteGoal);
		goalsTemplatesGroup.MapPost("/save/header", GoalsTemplatesEndpointsBehaviors.SaveHeaderChanges);

		var activeGoalsGroup = goalsGroup.MapGroup("/active");

		activeGoalsGroup.MapGet("/", GoalsEndpointsBehaviors.Goals);
		activeGoalsGroup.MapGet("/list/{goalId}", GoalsEndpointsBehaviors.GoalListItem);
		activeGoalsGroup.MapGet("/details/{goalId}", GoalsEndpointsBehaviors.GoalDetails);
		activeGoalsGroup.MapPost("/start", GoalsEndpointsBehaviors.StartTimer);
		activeGoalsGroup.MapPost("/stop", GoalsEndpointsBehaviors.StopTimer);
		activeGoalsGroup.MapPost("/create", GoalsEndpointsBehaviors.CreateGoal);
		activeGoalsGroup.MapPost("/persist", GoalsEndpointsBehaviors.PersistGoal);
		activeGoalsGroup.MapPost("/delete", GoalsEndpointsBehaviors.DeleteGoal);
		activeGoalsGroup.MapPost("/complete", GoalsEndpointsBehaviors.CompleteJob);
		activeGoalsGroup.MapPost("/save/header", GoalsEndpointsBehaviors.SaveHeaderChanges);
		activeGoalsGroup.MapPost("/time/add", GoalsEndpointsBehaviors.AddTimer);
		activeGoalsGroup.MapPost("/time/edit", GoalsEndpointsBehaviors.EditTimer);
		activeGoalsGroup.MapPost("/time/delete", GoalsEndpointsBehaviors.DeleteTimer);
	}
}

public static class GoalsTemplatesEndpointsBehaviors {
	public static async Task<IResult> Goals(
		[FromQuery] string userId,
		[FromServices] GoalsTemplatesRepository repository)
	{
		var data = (await repository.Templates(userId).ToListAsync()).Select(x => new GoalMainData(x)).ToList();
		return TypedResults.Json(data);
	}

	public static async Task<IResult> Goal(
		[FromRoute] int goalId,
		[FromServices] GoalsTemplatesRepository repository)
	{
		var template = await repository.Template(goalId);
		return template is null ? TypedResults.NotFound() : TypedResults.Json(new GoalMainData(template));
	}

	public static async Task<IResult> CreateGoal(
		[FromBody] GoalCreationData request,
		[FromServices] GoalsTemplatesRepository repository,
		[FromServices] IHubContext<GoalsHub, IGoalsClient> hubContext) 
	{
		GoalTemplate? template = await repository.Save(request.UserId, request.Name, request.Comment, request.ContractorId);
		if (template is null) return TypedResults.BadRequest();
		await hubContext.Clients.All.GoalTemplateCreated(request.UserId, template.Id, new GoalMainData(template));
		return TypedResults.Ok();
	}
	
	public static async Task<IResult> SaveHeaderChanges(
		[FromBody] GoalTemplateHeaderDataChanges request,
		[FromServices] GoalsTemplatesRepository repository,
		[FromServices] IHubContext<GoalsHub, IGoalsClient> hubContext)
	{
		GoalTemplate template = await repository.Update(request.Id, request.Name, request.Comment, request.ContractorId);
		GoalTemplateHeaderData data = new(template.Name, template.Comment, template.Contractor);
		await hubContext.Clients.All.GoalTemplateHeaderEdited(request.UserId, request.Id, data);
		return TypedResults.Ok();
	}

	public static async Task<IResult> DeleteGoal(
		[FromBody] GoalDeleteData request,
		[FromServices] GoalsTemplatesRepository repository,
		[FromServices] IHubContext<GoalsHub, IGoalsClient> hubContext) 
	{
		GoalTemplate? goal = await repository.Delete(request.GoalId);
		if (goal is null) return TypedResults.NotFound();
		await hubContext.Clients.All.GoalTemplateDeleted(request.UserId, request.GoalId);
		return TypedResults.Ok();
	}

	public static async Task<IResult> ActivateGoalByTemplate(
		[FromBody] GoalTemplateActivationData request,
		[FromServices] GoalsTemplatesRepository repository,
		[FromServices] GoalsRepository goalsRepository,
		[FromServices] IHubContext<GoalsHub, IGoalsClient> hubContext) 
	{
		Goal goal = await repository.Activate(request.TemplateId, request.ActivationDate);
		await hubContext.Clients.All.GoalActivated(goal.UserId, goal.Id);
		if (request.StartTimerImediate) {
			var timer = await goalsRepository.StartTimer(goal, request.ActivationDate);
			await hubContext.Clients.All.GoalActivated(goal.UserId, goal.Id, new(timer));
		}
		return TypedResults.Ok();
	}
}

public static class GoalsEndpointsBehaviors {
	public static async Task<IResult> Goals(
		[FromQuery] string userId,
		[FromServices] GoalsRepository goalsRepository)
	{
		var goals = (await goalsRepository.Goals(userId).ToListAsync()).Select(x => new ActiveGoalListData(x)).ToList();
		return TypedResults.Json(goals);
	}

	public static async Task<IResult> GoalListItem(
		[FromRoute] int goalId,
		[FromServices] GoalsRepository goalsRepository)
	{
		var goal = await goalsRepository.Goal(goalId);
		return goal is null ? TypedResults.NotFound() : TypedResults.Json(new ActiveGoalListData(goal));
	}

	public static async Task<IResult> GoalDetails(
		[FromRoute] int goalId,
		[FromServices] GoalsRepository goalsRepository) {
		var goal = await goalsRepository.Goal(goalId);
		return goal is null ? TypedResults.NotFound() : TypedResults.Json(new ActiveGoalDetailsData(goal));
	}

	public static async Task<IResult> CreateGoal(
		[FromBody] GoalCreationData request,
		[FromServices] GoalsRepository goalsRepository,
		[FromServices] IHubContext<GoalsHub, IGoalsClient> hubContext)
	{
		Goal goal = await goalsRepository.Create(request);
		await hubContext.Clients.All.GoalCreated(request.UserId, goal.Id, new ActiveGoalListData(goal));
		return TypedResults.Ok();
	}

	public static async Task<IResult> PersistGoal(
		[FromBody] GoalSaveAsTemplateData request,
		[FromServices] GoalsRepository repository,
		[FromServices] IHubContext<GoalsHub, IGoalsClient> hubContext)
	{
		GoalTemplate? goal = await repository.SaveAsTemplate(request);
		if (goal is null) return TypedResults.BadRequest();
		await hubContext.Clients.All.GoalTemplateCreated(request.UserId, goal.Id, new(goal));
		return TypedResults.Ok();
	}


	public static async Task<IResult> DeleteGoal(
		[FromBody] GoalDeleteData request,
		[FromServices] GoalsRepository goalsRepository,
		[FromServices] IHubContext<GoalsHub, IGoalsClient> hubContext) {
		Goal? goal = await goalsRepository.Delete(request.GoalId);
		if (goal is null) return TypedResults.NotFound();
		await hubContext.Clients.All.GoalDeleted(request.UserId, request.GoalId);
		return TypedResults.Ok();
	}

	public static async Task<IResult> StartTimer(
		[FromBody] GoalActivationData request,
		[FromServices] GoalsRepository activeGoalsRepository,
		[FromServices] IHubContext<GoalsHub, IGoalsClient> goalsHubContext) 
	{
		var timer = await activeGoalsRepository.StartTimer(request.GoalId, request.ActivationDate);
		await goalsHubContext.Clients.All.GoalActivated(request.UserId, request.GoalId, new(timer));
		return TypedResults.Ok();
	}

	public static async Task<IResult> StopTimer(
		[FromBody] GoalDeactivationData request,
		[FromServices] GoalsRepository activeGoalsRepository,
		[FromServices] IHubContext<GoalsHub, IGoalsClient> goalsHubContext) 
	{
		var goalTotalTime = await activeGoalsRepository.StopTimer(request.GoalId, request.DeactivationDate);
		await goalsHubContext.Clients.All.TimerStopped(request.UserId, request.GoalId, goalTotalTime);
		return TypedResults.Ok();
	}

	public static async Task<IResult> SaveHeaderChanges(
		[FromBody] GoalHeaderDataChanges request,
		[FromServices] GoalsRepository goalsRepository,
		[FromServices] IHubContext<GoalsHub, IGoalsClient> hubContext)
	{
		Goal goal = await goalsRepository.Update(request);
		ActiveGoalHeaderData data = new(goal.Name, goal.Comment, goal.Contractor);
		await hubContext.Clients.All.GoalHeaderEdited(request.UserId, request.Id, data);
		return TypedResults.Ok();
	}

	public static async Task<IResult> AddTimer(
		[FromBody] GoalTimeCreationData request,
		[FromServices] GoalsRepository goalsRepository,
		[FromServices] IHubContext<GoalsHub, IGoalsClient> hubContext)
	{
		(ActiveGoalTime GoalTime, TimeSpan GoalTotalTime)? result = await goalsRepository.CreateTimer(request);
		if (result is null) return TypedResults.Problem();
		await hubContext.Clients.All.GoalTimerCreated(request.UserId, request.GoalId, new(result.Value.GoalTime), result.Value.GoalTotalTime);
		return TypedResults.Ok();
	}
	
	public static async Task<IResult> EditTimer(
		[FromBody] GoalTimeEditData request,
		[FromServices] GoalsRepository goalsRepository,
		[FromServices] IHubContext<GoalsHub, IGoalsClient> hubContext)
	{
		(ActiveGoalTime GoalTime, TimeSpan GoalTotalTime)? result = await goalsRepository.EditTimer(request);
		if (result is null) return TypedResults.Problem();
		await hubContext.Clients.All.GoalTimerEdited(request.UserId, request.GoalId, new(result.Value.GoalTime), result.Value.GoalTotalTime);
		return TypedResults.Ok();
	}

	public static async Task<IResult> DeleteTimer(
		[FromBody] GoalTimeDeleteData request,
		[FromServices] GoalsRepository goalsRepository,
		[FromServices] IHubContext<GoalsHub, IGoalsClient> hubContext)
	{
		ActiveGoalTime? goalTIme = await goalsRepository.DeleteTimer(request.TimeId);
		if (goalTIme is null) return TypedResults.NotFound();
		await hubContext.Clients.All.GoalTimerDeleted(request.UserId, request.GoalId, request.TimeId, await goalsRepository.TotalTime(request.GoalId));
		return TypedResults.Ok();
	}


	public static async Task<IResult> CompleteJob(
		[FromBody] CompleteJobData request,
		[FromServices] GoalsRepository repository,
		[FromServices] IHubContext<GoalsHub, IGoalsClient> hubContext)
	{
		List<Goal> goals = await repository.CompleteJob(request.UserId, request.CompleteDate);
		
		await Parallel.ForEachAsync(goals, async (goal, token) =>
		{
			await hubContext.Clients.All.TimerStopped(request.UserId, goal.Id, goal.TotalTime());
		});
		return TypedResults.Ok();
	}
}