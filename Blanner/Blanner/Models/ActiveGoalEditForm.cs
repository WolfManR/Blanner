using Blanner.Data;
using Blanner.Hubs;

namespace Blanner.Models;

public class ActiveGoalEditForm() : GoalEditForm {
    public int? GoalId { get; set; }

    public string Comment { get; set; } = string.Empty;

    public int? ActiveTimerId { get; set; }

    public List<ActiveGoalTimeData> GoalTime { get; set; } = [];

    public void Init(ActiveGoalDetailsData? data = default) {
		Id = data?.Id ?? 0;
		Name = data?.Name ?? string.Empty;
		Contractor = data?.Contractor?.Id ?? 0;
		Comment = data?.Comment ?? string.Empty;
		GoalId = data?.GoalId;
		GoalTime = data?.GoalTime ?? [];
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
