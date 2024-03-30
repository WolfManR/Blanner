using Blanner.Data;
using Blanner.Extensions;

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;

namespace Blanner.Hubs.Clients;

public sealed class GoalsClient : HubClientBase, IGoalsClient {
	public GoalsClient(NavigationManager navigationManager) : base(navigationManager.GoalsHubUri()) {
		Hub.On<int, string, GoalData>(nameof(IGoalsClient.GoalCreated), GoalCreated);
		Hub.On<int, string, GoalHeaderData>(nameof(IGoalsClient.GoalHeaderEdited), GoalHeaderEdited);
		Hub.On<int, string>(nameof(IGoalsClient.GoalDeleted), GoalDeleted);
		
		Hub.On<int, int, string, ActiveGoalData>(nameof(IGoalsClient.GoalActivated), GoalActivated);
		Hub.On<int, string, ActiveGoalData>(nameof(IGoalsClient.ActiveGoalCreated), ActiveGoalCreated);
		Hub.On<int, string, ActiveGoalHeaderData>(nameof(IGoalsClient.ActiveGoalHeaderEdited), ActiveGoalHeaderEdited);
		Hub.On<int, string>(nameof(IGoalsClient.ActiveGoalDeleted), ActiveGoalDeleted);
		
		Hub.On<int, int, string, TimerEditData>(nameof(IGoalsClient.TimerEdited), TimerEdited);
		Hub.On<int, string, TimeSpan>(nameof(IGoalsClient.TimerStopped), TimerStopped);
		
		Hub.On<int, string, ActiveGoalTimeData, TimeSpan>(nameof(IGoalsClient.ActiveGoalTimerCreated), ActiveGoalTimerCreated);
		Hub.On<int, string, ActiveGoalTimeData, TimeSpan>(nameof(IGoalsClient.ActiveGoalTimerEdited), ActiveGoalTimerEdited);
		Hub.On<int, int, string, TimeSpan>(nameof(IGoalsClient.ActiveGoalTimerDeleted), ActiveGoalTimerDeleted);

		Hub.On<string>(nameof(IGoalsClient.JobsBuilded), JobsBuilded);
	}

    public event GoalCreated? OnGoalCreated;
	public event GoalHeaderEdited? OnGoalHeaderEdited;
	public event GoalDeleted? OnGoalDeleted;
   
	public event GoalActivated? OnGoalActivated;
	public event ActiveGoalCreated? OnActiveGoalCreated;
	public event ActiveGoalHeaderEdited? OnActiveGoalHeaderEdited;
	public event ActiveGoalDeleted? OnActiveGoalDeleted;
   
	public event TimerEdited? OnTimerEdited;
	public event TimerStopped? OnTimerStopped;
	
	public event ActiveGoalTimerCreated? OnActiveGoalTimerCreated;
	public event ActiveGoalTimerEdited? OnActiveGoalTimerEdited;
	public event ActiveGoalTimerDeleted? OnActiveGoalTimerDeleted;
	
	public event JobsBuilded? OnJobsBuilded;

	public async Task GoalCreated(int goalId, string userId, GoalData data) {
		if (OnGoalCreated is not null)
			await OnGoalCreated.Invoke(goalId, userId, data);
	}

	public async Task GoalHeaderEdited(int goalId, string userId, GoalHeaderData headerData) {
		if (OnGoalHeaderEdited is not null)
			await OnGoalHeaderEdited.Invoke(goalId, userId, headerData);
	}

	public async Task GoalDeleted(int goalId, string userId) {
		if (OnGoalDeleted is not null)
			await OnGoalDeleted.Invoke(goalId, userId);
	}


	public async Task GoalActivated(int goalId, int activeGoalId, string userId, ActiveGoalData data) {
		if (OnGoalActivated is not null)
			await OnGoalActivated.Invoke(goalId, activeGoalId, userId, data);
	}

	public async Task ActiveGoalCreated(int activeGoalId, string userId, ActiveGoalData data) {
		if (OnActiveGoalCreated is not null)
			await OnActiveGoalCreated.Invoke(activeGoalId, userId, data);
	}

	public async Task ActiveGoalHeaderEdited(int goalId, string userId, ActiveGoalHeaderData headerData) {
		if (OnActiveGoalHeaderEdited is not null)
			await OnActiveGoalHeaderEdited.Invoke(goalId, userId, headerData);
	}

	public async Task ActiveGoalDeleted(int goalId, string userId) {
		if (OnActiveGoalDeleted is not null)
			await OnActiveGoalDeleted.Invoke(goalId, userId);
	}


	public async Task TimerEdited(int goalId, int timerId, string userId, TimerEditData data) {
		if (OnTimerEdited is not null)
			await OnTimerEdited.Invoke(goalId, timerId, userId, data);
	}

	public async Task TimerStopped(int goalId, string userId, TimeSpan goalTimeTotal) {
		if (OnTimerStopped is not null)
			await OnTimerStopped.Invoke(goalId, userId, goalTimeTotal);
	}


	public async Task ActiveGoalTimerCreated(int goalId, string userId, ActiveGoalTimeData timerData, TimeSpan goalTimeTotal) {
		if (OnActiveGoalTimerCreated is not null)
			await OnActiveGoalTimerCreated.Invoke(goalId, userId, timerData, goalTimeTotal);
	}

	public async Task ActiveGoalTimerEdited(int goalId, string userId, ActiveGoalTimeData timerData, TimeSpan goalTimeTotal) {
		if (OnActiveGoalTimerEdited is not null)
			await OnActiveGoalTimerEdited.Invoke(goalId, userId, timerData, goalTimeTotal);
	}

	public async Task ActiveGoalTimerDeleted(int goalId, int timerId, string userId, TimeSpan goalTimeTotal) {
		if (OnActiveGoalTimerDeleted is not null)
			await OnActiveGoalTimerDeleted.Invoke(goalId, timerId, userId, goalTimeTotal);
	}


	public async Task JobsBuilded(string userId) {
		if (OnJobsBuilded is not null)
			await OnJobsBuilded.Invoke(userId);
	}
}
