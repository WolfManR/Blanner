﻿using Blanner.Data;
using Blanner.Data.Models;

using Microsoft.AspNetCore.SignalR;

namespace Blanner.Hubs;
public interface IGoalsClient {
	Task GoalCreated(int goalId, string userId, GoalData data);
	Task GoalHeaderEdited(int goalId, string userId, GoalHeaderData headerData);
	Task GoalDeleted(int goalId, string userId);
	
	Task GoalActivated(int goalId, int activeGoalId, string userId, ActiveGoalData data);
	Task ActiveGoalCreated(int activeGoalId, string userId, ActiveGoalData data);
	Task ActiveGoalHeaderEdited(int goalId, string userId, ActiveGoalHeaderData headerData);
	Task ActiveGoalDeleted(int goalId, string userId);
	
	Task TimerEdited(int goalId, int timerId, string userId, TimerEditData data);
	Task TimerStopped(int goalId, string userId, TimeSpan goalTimeTotal);

	Task ActiveGoalTimerCreated(int goalId, string userId, ActiveGoalTimeData timerData, TimeSpan goalTimeTotal);
	Task ActiveGoalTimerEdited(int goalId, string userId, ActiveGoalTimeData timerData, TimeSpan goalTimeTotal);
	Task ActiveGoalTimerDeleted(int goalId, int timerId, string userId, TimeSpan goalTimeTotal);

	Task JobsBuilded(string userId);
}

public class GoalsHub : Hub<IGoalsClient> {

}

public record ActiveGoalHeaderData(string Name, string Comment, Contractor? Contractor);
public record GoalHeaderData(string Name, Contractor? Contractor);
public record struct TimerEditData(DateTimeOffset Start, DateTimeOffset End, TimeSpan Time);