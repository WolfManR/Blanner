using Blanner.Data;
using Blanner.Extensions;

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;

namespace Blanner.Hubs.Clients;

public class JobsClient : HubClientBase, IJobsClient {
	public JobsClient(NavigationManager navigationManager) : base(navigationManager.JobsHubUri()) {
		Hub.On<int, string, JobEditableHeaderData>(nameof(IJobsClient.JobHeaderEdited), JobHeaderEdited);
		Hub.On<int, string, bool, int[]>(nameof(IJobsClient.JobStatusSavedEdited), JobStatusSavedEdited);
		Hub.On<int, string>(nameof(IJobsClient.JobDeleted), JobDeleted);

		Hub.On<string>(nameof(IJobsClient.JobsBuilded), JobsBuilded);
	}

	public event JobHeaderEdited? OnJobHeaderEdited;
	public event JobStatusSavedEdited? OnJobStatusSavedEdited;
	public event JobDeleted? OnJobDeleted;

	public event JobsBuilded? OnJobsBuilded;

	public async Task JobHeaderEdited(int jobId, string userId, JobEditableHeaderData headerData) {
		if (OnJobHeaderEdited is not null)
			await OnJobHeaderEdited.Invoke(jobId, userId, headerData);
	}

	public async Task JobStatusSavedEdited(int jobId, string userId, bool status, int[] updatedChanges) {
		if (OnJobStatusSavedEdited is not null)
			await OnJobStatusSavedEdited.Invoke(jobId, userId, status, updatedChanges);
	}

	public async Task JobDeleted(int jobId, string userId) {
		if (OnJobDeleted is not null)
			await OnJobDeleted.Invoke(jobId, userId);
	}

	public async Task JobsBuilded(string userId) {
		if (OnJobsBuilded is not null)
			await OnJobsBuilded.Invoke(userId);
	}
}
