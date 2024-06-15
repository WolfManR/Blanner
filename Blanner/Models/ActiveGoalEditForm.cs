using Blanner.Data;
using Blanner.Hubs;

namespace Blanner.Models;

public class ActiveGoalEditForm() : GoalTemplateEditForm {
    public int? ActiveTimerId { get; set; }

    public List<ActiveGoalTimeData> GoalTime { get; set; } = [];

    public void Init(ActiveGoalDetailsData? data = default) {
		Id = data?.Id ?? 0;
		Name = data?.Name ?? string.Empty;
		Comment = data?.Comment ?? string.Empty;
		Contractor = data?.Contractor?.Id ?? 0;
		GoalTime = data?.GoalTime ?? [];
	}

    public void Set(ActiveGoalHeaderData? data) {
        Name = data?.Name ?? string.Empty;
		Comment = data?.Comment ?? string.Empty;
		Contractor = data?.Contractor?.Id;
    }

    public void Deconstruct(out int id, out string name, out int? contractor, out string comment) {
        id = Id;
        name = Name;
        contractor = Contractor;
        comment = Comment;
    }
}
