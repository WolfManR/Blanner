using Blanner.Data;
using Blanner.Hubs;

namespace Blanner.Models;

public class GoalBaseEditForm() {
	public int Id { get; set; }
	public string Name { get; set; } = string.Empty;
	public string Comment { get; set; } = string.Empty;

	public bool IsEmpty { get; protected set; }
}

public class ActiveGoalEditForm() : GoalBaseEditForm {
	public int? ActiveTimerId { get; set; }
	public int? Contractor { get; set; }

	public List<ActiveGoalTimeData> GoalTime { get; set; } = [];

    public void Init(ActiveGoalDetailsData? data = default) {
		Id = data?.Id ?? 0;
		Name = data?.Name ?? string.Empty;
		Comment = data?.Comment ?? string.Empty;
		Contractor = data?.Contractor?.Id ?? 0;
		GoalTime = data?.GoalTime ?? [];

		IsEmpty = data is null;
	}

    public void Set(ActiveGoalHeaderData? data) {
        Name = data?.Name ?? string.Empty;
		Comment = data?.Comment ?? string.Empty;
		Contractor = data?.Contractor?.Id;

		IsEmpty = data is null;
	}

    public void Deconstruct(out int id, out string name, out int? contractor, out string comment) {
        id = Id;
        name = Name;
        contractor = Contractor;
        comment = Comment;
    }
}
