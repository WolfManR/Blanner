using Blanner.Data;
using Blanner.Data.Models;
using Blanner.Hubs;

namespace Blanner.Models.ViewModels;

public class ActiveGoalVM() {
	public int Id { get; private set; }
	public string Name { get; set; } = string.Empty;
	public string Comment { get; set; } = string.Empty;
	public Contractor? Contractor { get; set; }
	// ToDo
	public TimeSpan TotalElapsedTime { get; set; }
	public TimeSpan TimerTime { get; set; }
	public DateTimeOffset ActivationTime { get; set; }
	public int? ActiveTimerId { get; set; }

	public bool Tick { get; set; }

	public bool IsOnEdit { get; set; }

	public ActiveGoalVM(ActiveGoalListData data) : this() {
		Id = data.Id;
		Name = data.Name;
		Comment = data.Comment;
		Contractor = data.Contractor;
		TotalElapsedTime = data.TotalElapsedTime;
		ActivationTime = data.ActivationTime;
		ActiveTimerId = data.ActiveTimeId;
		Tick = data.ActiveTimeId is not null;
		TimerTime = Tick ? DateTimeOffset.Now - ActivationTime : TimeSpan.Zero;
	}

	public void Set(ActiveGoalHeaderData data) {
		Name = data.Name;
		Comment = data.Comment;
		Contractor = data.Contractor;
	}

	public void SetActiveData(ActiveGoalListData data) {
		TotalElapsedTime = data.TotalElapsedTime;
		ActivationTime = data.ActivationTime;
		ActiveTimerId = data.ActiveTimeId;
		Tick = data.ActiveTimeId is not null;
		TimerTime = Tick ? DateTimeOffset.Now - ActivationTime : TimeSpan.Zero;
	}

	public void TickTime() {
		TimerTime = Tick ? DateTimeOffset.Now - ActivationTime : TimeSpan.Zero;
	}
}