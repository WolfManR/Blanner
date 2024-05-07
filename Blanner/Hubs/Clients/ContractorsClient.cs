using Blanner.Extensions;

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;

namespace Blanner.Hubs.Clients;

public class ContractorsClient : HubClientBase, IContractorsClient {
	public ContractorsClient(NavigationManager navigationManager) : base(navigationManager.ContractorsHubUri()) {
		Hub.On<int, string>(nameof(IContractorsClient.ContractorCreated), ContractorCreated);
		Hub.On<int, string>(nameof(IContractorsClient.ContractorEdited), ContractorEdited);
		Hub.On<int>(nameof(IContractorsClient.ContractorDeleted), ContractorDeleted);
	}

	public event ContractorCreated? OnContractorCreated;
	public event ContractorEdited? OnContractorEdited;
	public event ContractorDeleted? OnContractorDeleted;

	public async Task ContractorCreated(int contractorId, string contractorName) {
		if (OnContractorCreated is not null)
			await OnContractorCreated.Invoke(contractorId, contractorName);
	}

	public async Task ContractorEdited(int contractorId, string contractorName) {
		if (OnContractorEdited is not null)
			await OnContractorEdited.Invoke(contractorId, contractorName);
	}

	public async Task ContractorDeleted(int contractorId) {
		if (OnContractorDeleted is not null)
			await OnContractorDeleted.Invoke(contractorId);
	}
}
