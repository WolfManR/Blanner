using Blanner.Data;
using Blanner.Hubs;

namespace Blanner.Models;

public class ActiveGoalEditForm() : GoalEditForm {
    private static ActiveGoalEditForm Empty { get; set; } = new();
    public new bool IsEmpty => ReferenceEquals(this, Empty);
    public int GoalId { get; set; }

    public string Comment { get; set; } = string.Empty;

    public int? ActiveTimerId { get; set; }

    public List<ActiveGoalTimeData> GoalTime { get; set; } = new();

    public static ActiveGoalEditForm Create(ActiveGoalDetailsData? data = default) {
        if (data is null) return Empty;
        return new() {
            Id = data.Id,
            Name = data.Name,
            Contractor = data.Contractor?.Id ?? 0,
            Comment = data.Comment,
            GoalId = data.GoalId,
            GoalTime = data.GoalTime
        };
    }

    public override void Set(ActiveGoalHeaderData data) {
        base.Set(data);
        Comment = data.Comment;
    }

    public void Deconstruct(out int id, out string name, out int? contractor, out string comment) {
        id = Id;
        name = Name;
        contractor = Contractor;
        comment = Comment;
    }
}
