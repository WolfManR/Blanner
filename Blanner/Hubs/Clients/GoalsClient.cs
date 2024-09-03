using Blanner.Data;
using Blanner.Extensions;

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;

namespace Blanner.Hubs.Clients;

public sealed class GoalsClient : HubClientBase, IGoalsClient {
	public GoalsClient(NavigationManager navigationManager) : base(navigationManager.GoalsHubUri()) {
		Hub.On<GoalTemplateCreationEventArgs>(nameof(IGoalsClient.GoalTemplateCreated), GoalTemplateCreated);
		Hub.On<string, int, GoalTemplateHeaderData>(nameof(IGoalsClient.GoalTemplateHeaderEdited), GoalTemplateHeaderEdited);
		Hub.On<string, int>(nameof(IGoalsClient.GoalTemplateDeleted), GoalTemplateDeleted);
		
		Hub.On<string, int, ActiveGoalTimeData?>(nameof(IGoalsClient.GoalActivated), GoalActivated);
		Hub.On<string, int, ActiveGoalListData>(nameof(IGoalsClient.GoalCreated), GoalCreated);
		Hub.On<string, int, ActiveGoalHeaderData>(nameof(IGoalsClient.GoalHeaderEdited), GoalHeaderEdited);
		Hub.On<string, int>(nameof(IGoalsClient.GoalDeleted), GoalDeleted);
		
		Hub.On<string, int, int, TimeEditData>(nameof(IGoalsClient.TimeEdited), TimeEdited);
		Hub.On<string, int, TimeSpan>(nameof(IGoalsClient.TimeStopped), TimeStopped);
		
		Hub.On<string, int, ActiveGoalTimeData, TimeSpan>(nameof(IGoalsClient.GoalTimeCreated), GoalTimeCreated);
		Hub.On<string, int, ActiveGoalTimeData, TimeSpan>(nameof(IGoalsClient.GoalTimeEdited), GoalTimeEdited);
		Hub.On<string, int, int, TimeSpan>(nameof(IGoalsClient.GoalTimeDeleted), GoalTimeDeleted);

		Hub.On<string>(nameof(IGoalsClient.JobsBuilded), JobsBuilded);
	}

    public event GoalTemplateCreated? OnGoalTemplateCreated;
	public event GoalTemplateHeaderEdited? OnGoalTemplateHeaderEdited;
	public event GoalTemplateDeleted? OnGoalTemplateDeleted;
   
	public event GoalActivated? OnGoalActivated;
	public event GoalCreated? OnGoalCreated;
	public event GoalHeaderEdited? OnGoalHeaderEdited;
	public event GoalDeleted? OnGoalDeleted;
   
	public event TimeEdited? OnTimeEdited;
	public event TimeStopped? OnTimeStopped;
	
	public event GoalTimeCreated? OnGoalTimeCreated;
	public event GoalTimeEdited? OnGoalTimeEdited;
	public event GoalTimeDeleted? OnGoalTimeDeleted;
	
	public event JobsBuilded? OnJobsBuilded;

	public async Task GoalTemplateCreated(GoalTemplateCreationEventArgs eventArgs) {
		if (OnGoalTemplateCreated is not null)
			await OnGoalTemplateCreated.Invoke(eventArgs);
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


	public async Task TimeEdited(string userId, int goalId, int timeId, TimeEditData data) {
		if (OnTimeEdited is not null)
			await OnTimeEdited.Invoke(userId, goalId, timeId, data);
	}

	public async Task TimeStopped(string userId, int goalId, TimeSpan goalTimeTotal) {
		if (OnTimeStopped is not null)
			await OnTimeStopped.Invoke(userId, goalId, goalTimeTotal);
	}


	public async Task GoalTimeCreated(string userId, int goalId, ActiveGoalTimeData timerData, TimeSpan goalTimeTotal) {
		if (OnGoalTimeCreated is not null)
			await OnGoalTimeCreated.Invoke(userId, goalId, timerData, goalTimeTotal);
	}

	public async Task GoalTimeEdited(string userId, int goalId, ActiveGoalTimeData timerData, TimeSpan goalTimeTotal) {
		if (OnGoalTimeEdited is not null)
			await OnGoalTimeEdited.Invoke(userId, goalId, timerData, goalTimeTotal);
	}

	public async Task GoalTimeDeleted(string userId, int goalId, int timeId, TimeSpan goalTimeTotal) {
		if (OnGoalTimeDeleted is not null)
			await OnGoalTimeDeleted.Invoke(userId, goalId, timeId, goalTimeTotal);
	}


	public async Task JobsBuilded(string userId) {
		if (OnJobsBuilded is not null)
			await OnJobsBuilded.Invoke(userId);
	}
}
