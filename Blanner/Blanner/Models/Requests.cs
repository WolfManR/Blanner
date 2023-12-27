namespace Blanner.Models;

public sealed record ContractorCreateRequest(string Name, DateTimeOffset CreatedAt);
public sealed record ContractorEditRequest(int Id, string Name, DateTimeOffset UpdatedAt);

public sealed record TimerActivationData(int? GoalId, int? ActiveGoalId, DateTimeOffset ActivationDate, string UserId);
public sealed record TimerDeactivationData(int GoalId, string UserId, DateTimeOffset StopDate);
public sealed record GoalPushToActiveData(int GoalId, string UserId, DateTimeOffset ActivationDate);
public sealed record GoalHeaderChangesSaveData(int GoalId, string UserId, string Name, int? ContractorId);
public sealed record GoalCreationData(string Name, string UserId);
public sealed record GoalDeleteData(int GoalId, string UserId);
public sealed record GoalTimeDeleteData(int GoalId, int TimeId, string UserId);
public sealed record ActiveGoalHeaderChangesSaveData(int GoalId, string UserId, string Name, int? ContractorId, string Comment);
public sealed record GoalTimeCreationData(int GoalId, string UserId, DateTimeOffset Start, DateTimeOffset? End, TimeSpan? Time);
public sealed record GoalTimeEditData(int GoalId, int TimerId, string UserId, DateTimeOffset Start, DateTimeOffset? End, TimeSpan? Time);

public sealed record CompleteJobData(string UserId, DateTimeOffset CompleteDate);
public sealed record JobsListData(string UserId, DateTimeOffset Start, DateTimeOffset End);
public sealed record BuildJobData(string UserId, DateTimeOffset BuildDate);