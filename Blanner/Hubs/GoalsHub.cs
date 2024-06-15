using Blanner.Data;
using Blanner.Data.Models;

using Microsoft.AspNetCore.SignalR;

namespace Blanner.Hubs;
public interface IGoalsClient {
	Task GoalTemplateCreated(string userId, int goalId, GoalMainData data);
	Task GoalTemplateHeaderEdited(string userId, int goalId, GoalTemplateHeaderData headerData);
	Task GoalTemplateDeleted(string userId, int goalId);
	
	Task GoalActivated(string userId, int goalId, ActiveGoalTimeData? timerData = null);
	Task GoalCreated(string userId, int goalId, ActiveGoalListData data);
	Task GoalHeaderEdited(string userId, int goalId, ActiveGoalHeaderData headerData);
	Task GoalDeleted(string userId, int goalId);
	
	Task TimerEdited(string userId, int goalId, int timerId, TimerEditData data);
	Task TimerStopped(string userId, int goalId, TimeSpan goalTimeTotal);

	Task GoalTimerCreated(string userId, int goalId, ActiveGoalTimeData timerData, TimeSpan goalTimeTotal);
	Task GoalTimerEdited(string userId, int goalId, ActiveGoalTimeData timerData, TimeSpan goalTimeTotal);
	Task GoalTimerDeleted(string userId, int goalId, int timerId, TimeSpan goalTimeTotal);

	Task JobsBuilded(string userId);
}

public delegate Task GoalTemplateCreated(string userId, int goalId, GoalMainData data);
public delegate Task GoalTemplateHeaderEdited(string userId, int goalId, GoalTemplateHeaderData headerData);
public delegate Task GoalTemplateDeleted(string userId, int goalId);

public delegate Task GoalActivated(string userId, int goalId, ActiveGoalTimeData? timerData = null);
public delegate Task GoalCreated(string userId, int goalId, ActiveGoalListData data);
public delegate Task GoalHeaderEdited(string userId, int goalId, ActiveGoalHeaderData headerData);
public delegate Task GoalDeleted(string userId, int goalId);

public delegate Task TimerEdited(string userId, int goalId, int timerId, TimerEditData data);
public delegate Task TimerStopped(string userId, int goalId, TimeSpan goalTimeTotal);

public delegate Task GoalTimerCreated(string userId, int goalId, ActiveGoalTimeData timerData, TimeSpan goalTimeTotal);
public delegate Task GoalTimerEdited(string userId, int goalId, ActiveGoalTimeData timerData, TimeSpan goalTimeTotal);
public delegate Task GoalTimerDeleted(string userId, int goalId, int timerId, TimeSpan goalTimeTotal);

public delegate Task JobsBuilded(string userId);

public class GoalsHub : Hub<IGoalsClient> {

}

public record ActiveGoalHeaderData(string Name, string Comment, Contractor? Contractor);
public record GoalTemplateHeaderData(string Name, string Comment, Contractor? Contractor);
public record struct TimerEditData(DateTimeOffset Start, DateTimeOffset End, TimeSpan Time);