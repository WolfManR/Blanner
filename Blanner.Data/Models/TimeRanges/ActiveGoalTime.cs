using Blanner.Data.DataModels;
using Blanner.Data.Models.WorksManagement;

namespace Blanner.Data.Models.TimeRanges;
public class ActiveGoalTime : TimeRange, ITime
{
    public long Id { get; set; }
    public int ActiveGoalId { get; set; }

    public Goal ActiveGoal { get; init; } = null!;
}
