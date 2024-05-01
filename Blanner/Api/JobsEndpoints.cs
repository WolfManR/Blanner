using Blanner.Data;
using Microsoft.AspNetCore.Mvc;
using Blanner.Hubs;
using Microsoft.AspNetCore.SignalR;

namespace Blanner.Api;

public static class JobsEndpoints {
	public static void MapJobs(this IEndpointRouteBuilder endpoints) {
		var jobsGroup = endpoints.MapGroup("/jobs");

		jobsGroup.MapPost("/", JobsEndpointsBehaviors.Jobs);
		jobsGroup.MapGet("/{jobId}", JobsEndpointsBehaviors.Job);
		jobsGroup.MapPost("/build", JobsEndpointsBehaviors.BuildJob);
		jobsGroup.MapPost("/save", JobsEndpointsBehaviors.SaveHeaderData);
		jobsGroup.MapPost("/update-status/saved", JobsEndpointsBehaviors.UpdateStatusSaved);
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
		[FromServices] JobsRepository repository,
		[FromServices] IHubContext<GoalsHub, IGoalsClient> goalsHubContext,
		[FromServices] IHubContext<JobsHub, IJobsClient> jobsHubContext) {
		var success = await repository.BuildJob(request);
		if (!success) return TypedResults.BadRequest();

		await goalsHubContext.Clients.All.JobsBuilded(request.UserId);
		await jobsHubContext.Clients.All.JobsBuilded(request.UserId);
		return TypedResults.Ok();
	}

	public static async Task<IResult> SaveHeaderData(
		[FromBody] JobHeaderSaveData request,
		[FromServices] JobsRepository repository,
		[FromServices] IHubContext<JobsHub, IJobsClient> hubContext) {
		var context = await repository.Update(request);
		if (context is null) return TypedResults.BadRequest();

		await hubContext.Clients.All.JobHeaderEdited(context.Id, request.UserId, new() { User = context.User, Name = context.Name, Contractor = context.Contractor, Comment = context.Comment });
		return TypedResults.Ok();
	}

	public static async Task<IResult> UpdateStatusSaved(
		[FromBody] JobSavedChangedData request,
		[FromServices] JobsRepository repository,
		[FromServices] IHubContext<JobsHub, IJobsClient> hubContext) {
		var context = await repository.Update(request);
		if (context is null) return TypedResults.BadRequest();

		await hubContext.Clients.All.JobStatusSavedEdited(context.Id, request.UserId, context.Saved, context.Changes.Select(x => x.Id).ToArray());
		return TypedResults.Ok();
	}
}

