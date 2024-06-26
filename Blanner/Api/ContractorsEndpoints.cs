﻿using Blanner.Data;
using Blanner.Data.Models;
using Blanner.Hubs;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace Blanner.Api;
public static class ContractorsEndpoints {
    public static void MapContractors(this IEndpointRouteBuilder endpoints) {
        var contractorsGroup = endpoints.MapGroup("/contractors");

        contractorsGroup.MapGet("/", ContractorsEndpointsBehaviors.Contractors);
        contractorsGroup.MapGet("/{contractorId}", ContractorsEndpointsBehaviors.Contractor);
        contractorsGroup.MapPost("/", ContractorsEndpointsBehaviors.Add);
        contractorsGroup.MapPut("/", ContractorsEndpointsBehaviors.Save);
        contractorsGroup.MapDelete("/{contractorId}", ContractorsEndpointsBehaviors.Delete);
    }
}

public static class ContractorsEndpointsBehaviors {
    public static async Task<IResult> Contractors([FromServices] ContractorsRepository contractorsRepository) {
        var data = await contractorsRepository.List();
        return TypedResults.Json(data);
    }

	public static async Task<IResult> Contractor([FromRoute] int contractorId, [FromServices] ContractorsRepository contractorsRepository) {
		var data = await contractorsRepository.Contractor(contractorId);
		return TypedResults.Json(data);
	}

	public static async Task<IResult> Add(
        [FromBody] ContractorCreateRequest request,
        [FromServices] ContractorsRepository contractorsRepository,
        [FromServices] IHubContext<ContractorsHub, IContractorsClient> contractorsHubContext) {
        Contractor data = await contractorsRepository.Add(request.Name, request.CreatedAt);
        await contractorsHubContext.Clients.All.ContractorCreated(data.Id, data.Name);
        return TypedResults.Json(data);
    }

    public static async Task<IResult> Save(
        [FromBody] ContractorEditRequest request,
        [FromServices] ContractorsRepository contractorsRepository,
		[FromServices] IHubContext<ContractorsHub, IContractorsClient> contractorsHubContext) {
        Contractor? data = await contractorsRepository.Save(request.Id, request.Name, request.UpdatedAt);
        if (data is null) return TypedResults.NotFound();
		await contractorsHubContext.Clients.All.ContractorEdited(data.Id, data.Name);
		return TypedResults.Json(data);
    }

    public static async Task<IResult> Delete(
        [FromRoute] int contractorId,
        [FromServices] ContractorsRepository contractorsRepository,
		[FromServices] IHubContext<ContractorsHub, IContractorsClient> contractorsHubContext) {
        Contractor? data = await contractorsRepository.Destroy(contractorId);
        if (data is null) return TypedResults.NotFound();
		await contractorsHubContext.Clients.All.ContractorDeleted(data.Id);
		return TypedResults.Json(data);
    }
}
