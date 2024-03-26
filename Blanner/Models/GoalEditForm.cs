using Blanner.Data;
using Blanner.Hubs;

namespace Blanner.Models;

public class GoalEditForm() {
    private static GoalEditForm Empty { get; set; } = new();
    public bool IsEmpty => ReferenceEquals(this, Empty);

    public int Id { get; set; }
    public int? ActiveGoalId { get; set; }

    public string Name { get; set; } = string.Empty;
    public int? Contractor { get; set; }

    public static GoalEditForm Create(GoalDetailsData? data = default) {
        if (data is null) return Empty;
        return new() {
            Id = data.Id,
            Name = data.Name,
            Contractor = data.Contractor?.Id ?? 0,
            ActiveGoalId = data.ActiveGoalId
        };
    }

    public void Set(GoalHeaderData data) {
        Name = data.Name;
        Contractor = data.Contractor?.Id;
    }
    public virtual void Set(ActiveGoalHeaderData data) {
        Name = data.Name;
        Contractor = data.Contractor?.Id;
    }

    public void Deconstruct(out int id, out string name, out int? contractor) {
        id = Id;
        name = Name;
        contractor = Contractor;
    }
}
