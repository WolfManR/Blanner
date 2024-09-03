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
	
	Task TimeEdited(string userId, int goalId, int timeId, TimeEditData data);
	Task TimeStopped(string userId, int goalId, TimeSpan goalTimeTotal);

	Task GoalTimeCreated(string userId, int goalId, ActiveGoalTimeData timerData, TimeSpan goalTimeTotal);
	Task GoalTimeEdited(string userId, int goalId, ActiveGoalTimeData timerData, TimeSpan goalTimeTotal);
	Task GoalTimeDeleted(string userId, int goalId, int timeId, TimeSpan goalTimeTotal);

	Task JobsBuilded(string userId);
}

public delegate Task GoalTemplateCreated(string userId, int goalId, GoalMainData data);
public delegate Task GoalTemplateHeaderEdited(string userId, int goalId, GoalTemplateHeaderData headerData);
public delegate Task GoalTemplateDeleted(string userId, int goalId);

public delegate Task GoalActivated(string userId, int goalId, ActiveGoalTimeData? timerData = null);
public delegate Task GoalCreated(string userId, int goalId, ActiveGoalListData data);
public delegate Task GoalHeaderEdited(string userId, int goalId, ActiveGoalHeaderData headerData);
public delegate Task GoalDeleted(string userId, int goalId);

public delegate Task TimeEdited(string userId, int goalId, int timeId, TimeEditData data);
public delegate Task TimeStopped(string userId, int goalId, TimeSpan goalTimeTotal);

public delegate Task GoalTimeCreated(string userId, int goalId, ActiveGoalTimeData timerData, TimeSpan goalTimeTotal);
public delegate Task GoalTimeEdited(string userId, int goalId, ActiveGoalTimeData timerData, TimeSpan goalTimeTotal);
public delegate Task GoalTimeDeleted(string userId, int goalId, int timeId, TimeSpan goalTimeTotal);

public delegate Task JobsBuilded(string userId);

public class GoalsHub : Hub<IGoalsClient> {

}

public record ActiveGoalHeaderData(string Name, string Comment, Contractor? Contractor);
public record GoalTemplateHeaderData(string Name, string Comment, Contractor? Contractor);
public record struct TimeEditData(DateTimeOffset Start, DateTimeOffset End, TimeSpan Time);