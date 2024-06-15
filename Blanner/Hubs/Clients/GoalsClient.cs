using Blanner.Data;
using Blanner.Extensions;

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;

namespace Blanner.Hubs.Clients;

public sealed class GoalsClient : HubClientBase, IGoalsClient {
	public GoalsClient(NavigationManager navigationManager) : base(navigationManager.GoalsHubUri()) {
		Hub.On<string, int, GoalMainData>(nameof(IGoalsClient.GoalTemplateCreated), GoalTemplateCreated);
		Hub.On<string, int, GoalTemplateHeaderData>(nameof(IGoalsClient.GoalTemplateHeaderEdited), GoalTemplateHeaderEdited);
		Hub.On<string, int>(nameof(IGoalsClient.GoalTemplateDeleted), GoalTemplateDeleted);
		
		Hub.On<string, int, ActiveGoalTimeData?>(nameof(IGoalsClient.GoalActivated), GoalActivated);
		Hub.On<string, int, ActiveGoalListData>(nameof(IGoalsClient.GoalCreated), GoalCreated);
		Hub.On<string, int, ActiveGoalHeaderData>(nameof(IGoalsClient.GoalHeaderEdited), GoalHeaderEdited);
		Hub.On<string, int>(nameof(IGoalsClient.GoalDeleted), GoalDeleted);
		
		Hub.On<string, int, int, TimerEditData>(nameof(IGoalsClient.TimerEdited), TimerEdited);
		Hub.On<string, int, TimeSpan>(nameof(IGoalsClient.TimerStopped), TimerStopped);
		
		Hub.On<string, int, ActiveGoalTimeData, TimeSpan>(nameof(IGoalsClient.GoalTimerCreated), GoalTimerCreated);
		Hub.On<string, int, ActiveGoalTimeData, TimeSpan>(nameof(IGoalsClient.GoalTimerEdited), GoalTimerEdited);
		Hub.On<string, int, int, TimeSpan>(nameof(IGoalsClient.GoalTimerDeleted), GoalTimerDeleted);

		Hub.On<string>(nameof(IGoalsClient.JobsBuilded), JobsBuilded);
	}

    public event GoalTemplateCreated? OnGoalTemplateCreated;
	public event GoalTemplateHeaderEdited? OnGoalTemplateHeaderEdited;
	public event GoalTemplateDeleted? OnGoalTemplateDeleted;
   
	public event GoalActivated? OnGoalActivated;
	public event GoalCreated? OnGoalCreated;
	public event GoalHeaderEdited? OnGoalHeaderEdited;
	public event GoalDeleted? OnGoalDeleted;
   
	public event TimerEdited? OnTimerEdited;
	public event TimerStopped? OnTimerStopped;
	
	public event GoalTimerCreated? OnGoalTimerCreated;
	public event GoalTimerEdited? OnGoalTimerEdited;
	public event GoalTimerDeleted? OnGoalTimerDeleted;
	
	public event JobsBuilded? OnJobsBuilded;

	public async Task GoalTemplateCreated(string userId, int goalId, GoalMainData data) {
		if (OnGoalTemplateCreated is not null)
			await OnGoalTemplateCreated.Invoke(userId, goalId, data);
	}

	public async Task GoalTemplateHeaderEdited(string userId, int goalId, GoalTemplateHeaderData headerData) {
		if (OnGoalTemplateHeaderEdited is not null)
			await OnGoalTemplateHeaderEdited.Invoke(userId, goalId, headerData);
	}

	public async Task GoalTemplateDeleted(string userId, int goalId) {
		if (OnGoalTemplateDeleted is not null)
			await OnGoalTemplateDeleted.Invoke(userId, goalId);
	}


	public async Task GoalActivated(string userId, int goalId, ActiveGoalTimeData? timerData) {
		if (OnGoalActivated is not null)
			await OnGoalActivated.Invoke(userId, goalId, timerData);
	}

	public async Task GoalCreated(string userId, int goalId, ActiveGoalListData data) {
		if (OnGoalCreated is not null)
			await OnGoalCreated.Invoke(userId, goalId, data);
	}

	public async Task GoalHeaderEdited(string userId, int goalId, ActiveGoalHeaderData headerData) {
		if (OnGoalHeaderEdited is not null)
			await OnGoalHeaderEdited.Invoke(userId, goalId, headerData);
	}

	public async Task GoalDeleted(string userId, int goalId) {
		if (OnGoalDeleted is not null)
			await OnGoalDeleted.Invoke(userId, goalId);
	}


	public async Task TimerEdited(string userId, int goalId, int timerId, TimerEditData data) {
		if (OnTimerEdited is not null)
			await OnTimerEdited.Invoke(userId, goalId, timerId, data);
	}

	public async Task TimerStopped(string userId, int goalId, TimeSpan goalTimeTotal) {
		if (OnTimerStopped is not null)
			await OnTimerStopped.Invoke(userId, goalId, goalTimeTotal);
	}


	public async Task GoalTimerCreated(string userId, int goalId, ActiveGoalTimeData timerData, TimeSpan goalTimeTotal) {
		if (OnGoalTimerCreated is not null)
			await OnGoalTimerCreated.Invoke(userId, goalId, timerData, goalTimeTotal);
	}

	public async Task GoalTimerEdited(string userId, int goalId, ActiveGoalTimeData timerData, TimeSpan goalTimeTotal) {
		if (OnGoalTimerEdited is not null)
			await OnGoalTimerEdited.Invoke(userId, goalId, timerData, goalTimeTotal);
	}

	public async Task GoalTimerDeleted(string userId, int goalId, int timerId, TimeSpan goalTimeTotal) {
		if (OnGoalTimerDeleted is not null)
			await OnGoalTimerDeleted.Invoke(userId, goalId, timerId, goalTimeTotal);
	}


	public async Task JobsBuilded(string userId) {
		if (OnJobsBuilded is not null)
			await OnJobsBuilded.Invoke(userId);
	}
}
