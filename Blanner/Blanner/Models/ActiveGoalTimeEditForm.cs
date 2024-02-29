using Blanner.Data;

namespace Blanner.Models;

public class ActiveGoalTimeEditForm {
    public int Id { get; set; }
    public DateTimeOffset Start { get; set; }
    public DateTimeOffset End { get; set; }

    public static ActiveGoalTimeEditForm Create(ActiveGoalTimeData? data = default) {
        if (data is null) {
            var rangeValue = DateTimeOffset.Now;
            return new() {
                Start = rangeValue,
                End = rangeValue
            };
        }

        return new() {
            Id = data.Id,
            Start = data.Start,
            End = data.End
        };
    }
}