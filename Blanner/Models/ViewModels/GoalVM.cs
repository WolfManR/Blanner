using Blanner.Data.Models;
using Blanner.Data;
using Blanner.Hubs;

namespace Blanner.Models.ViewModels;

public class GoalVM() {
	public int Id { get; private set; }
	public string Name { get; set; } = string.Empty;
	public Contractor? Contractor { get; set; }
	public int? ActiveGoalId { get; set; }
	// ToDo

	public bool IsOnEdit { get; set; }

	public GoalVM(GoalData data) : this() {
		Id = data.Id;
		Name = data.Name;
		Contractor = data.Contractor;
		ActiveGoalId = data.ActiveGoalId;
	}

	public void Set(ActiveGoalHeaderData data) {
		Name = data.Name;
		Contractor = data.Contractor;
	}
	public void Set(GoalHeaderData data) {
		Name = data.Name;
		Contractor = data.Contractor;
	}

	public void SetActiveData(ActiveGoalData data) {
		ActiveGoalId = data.Id;
	}
}