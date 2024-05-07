using Microsoft.AspNetCore.SignalR;

namespace Blanner.Hubs;

public interface IContractorsClient
{
    Task ContractorCreated(int contractorId, string contractorName);
    Task ContractorEdited(int contractorId, string contractorName);
    Task ContractorDeleted(int contractorId);
}

public delegate Task ContractorCreated(int contractorId, string contractorName);
public delegate Task ContractorEdited(int contractorId, string contractorName);
public delegate Task ContractorDeleted(int contractorId);

public class ContractorsHub : Hub<IContractorsClient> {
}
