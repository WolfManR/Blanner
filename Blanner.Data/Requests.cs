namespace Blanner.Data;

public sealed record ContractorCreateRequest(string Name, DateTimeOffset CreatedAt);
public sealed record ContractorEditRequest(int Id, string Name, DateTimeOffset UpdatedAt);

public sealed record GoalTemplateCreationData(string UserId, string Name, string Comment, int[] ContractorsId);
public sealed record GoalTemplateHeaderDataChanges(string UserId, int Id, string Name, string Comment, int[] ContractorsId);
public sealed record GoalTemplateActivationData(string UserId, int TemplateId, DateTimeOffset ActivationDate, bool StartTimerImediate = false);

public sealed record GoalCreationData(string UserId, string Name, string Comment, int? ContractorId);
public sealed record GoalHeaderDataChanges(string UserId, int Id, string Name, string Comment, int? ContractorId);
public sealed record GoalDeleteData(string UserId, int GoalId);
public sealed record GoalSaveAsTemplateData(int GoalId, string UserId);

public sealed record GoalActivationData(string UserId, int GoalId, DateTimeOffset ActivationDate);
public sealed record GoalDeactivationData(string UserId, int GoalId,  DateTimeOffset DeactivationDate);

public sealed record GoalTimeCreationData(int GoalId, string UserId, DateTimeOffset Start, DateTimeOffset? End, TimeSpan? Time);
public sealed record GoalTimeEditData(int GoalId, int TimeId, string UserId, DateTimeOffset Start, DateTimeOffset? End, TimeSpan? Time);
public sealed record GoalTimeDeleteData(int GoalId, int TimeId, string UserId);

public sealed record CompleteJobData(string UserId, DateTimeOffset CompleteDate);
public sealed record JobsListData(string UserId, DateOnly Start, DateOnly End);
public sealed record BuildJobData(string UserId, DateTimeOffset BuildDate);
public sealed record JobHeaderSaveData(int JobId, string UserId, string Name, int? ContractorId, string Comment = "");
public sealed record JobSavedChangedData(int JobId, string UserId, bool Saved);