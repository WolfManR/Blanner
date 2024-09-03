using Blanner.Data.DataModels;
using Blanner.Data.Models.WorksManagement;

namespace Blanner.Data.Models.TimeRanges;
public class ActiveGoalTime : TimeRange, ITime
{
    public int Id { get; set; }
    public int ActiveGoalId { get; set; }

    public Goal ActiveGoal { get; init; } = null!;
}
