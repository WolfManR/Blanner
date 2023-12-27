using Blanner.Data.DataModels;

namespace Blanner.Data.Models;
public class ActiveGoalTime : ITime {
    public int Id { get; set; }
    public DateTimeOffset Start { get; set; }
    public DateTimeOffset End { get; set; }

    public ActiveGoal ActiveGoal { get; init; } = null!;
}
