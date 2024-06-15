using Blanner.Data.Models;
using Blanner.Data;
using Blanner.Hubs;

namespace Blanner.Models.ViewModels;

public class GoalTemplateVM() {
	public int Id { get; private set; }
	public string Name { get; set; } = string.Empty;
	public string Comment { get; set; } = string.Empty;
	public Contractor? Contractor { get; set; }
	// ToDo

	public bool IsOnEdit { get; set; }

	public GoalTemplateVM(GoalMainData data) : this() {
		Id = data.Id;
		Name = data.Name;
		Comment = data.Comment;
		Contractor = data.Contractor;
	}

	public void Set(GoalTemplateHeaderData data) {
		Name = data.Name;
		Comment = data.Comment;
		Contractor = data.Contractor;
	}
}