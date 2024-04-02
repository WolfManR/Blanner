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
		[FromServices] JobsRepository repository,
		[FromServices] IHubContext<GoalsHub, IGoalsClient> hubContext) {
		var success = await repository.BuildJob(request);
		if (!success) return TypedResults.BadRequest();

		await hubContext.Clients.All.JobsBuilded(request.UserId);
		return TypedResults.Ok();
	}
}

