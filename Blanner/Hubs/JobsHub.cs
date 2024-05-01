using Blanner.Data;
using Microsoft.AspNetCore.SignalR;

namespace Blanner.Hubs;
public interface IJobsClient {
	Task JobHeaderEdited(int jobId, string userId, JobEditableHeaderData headerData);
	Task JobStatusSavedEdited(int jobId, string userId, bool status, int[] updatedChanges);
	Task JobDeleted(int jobId, string userId);

	//Task TimerEdited(int jobId, int timerId, string userId, TimerEditData data);

	//Task JobTimerCreated(int jobId, string userId, JobTimeData timerData, TimeSpan jobTimeTotal);
	//Task JobTimerEdited(int jobId, string userId, JobTimeData timerData, TimeSpan jobTimeTotal);
	//Task JobTimerDeleted(int jobId, int timerId, string userId, TimeSpan jobTimeTotal);

	Task JobsBuilded(string userId);
}

public delegate Task JobHeaderEdited(int jobId, string userId, JobEditableHeaderData headerData);
public delegate Task JobStatusSavedEdited(int jobId, string userId, bool status, int[] updatedChanges);
public delegate Task JobDeleted(int jobId, string userId);

//public delegate Task TimerEdited(int jobId, int timerId, string userId, TimerEditData data);

//public delegate Task JobTimerCreated(int jobId, string userId, JobTimeData timerData, TimeSpan goalTimeTotal);
//public delegate Task JobTimerEdited(int jobId, string userId, JobTimeData timerData, TimeSpan goalTimeTotal);
//public delegate Task JobTimerDeleted(int jobId, int timerId, string userId, TimeSpan goalTimeTotal);

public class JobsHub : Hub<IJobsClient> {

}
