namespace Blanner.Data.Models; 
public class ActiveGoal() : BaseModel
{
    public ActiveGoal(Goal goal, DateTimeOffset createDate) : this()
    {
        CreatedAt = createDate;
        Goal = goal;
        Name = goal.Name;
        Contractor = goal.Contractor;
        User = goal.User;
        Tasks = goal.Tasks;
    }

    public string Name { get; set; } = string.Empty;
    public string Comment { get; set; } = string.Empty;
    public bool CreateGoalOnAssemblingJob { get; set; }

    public List<ToDo> Tasks { get; set; } = [];
    public List<ActiveGoalTime> GoalTime { get; set; } = [];
    public int? CurrentlyActiveTime { get; set; }

    public User? User { get; set; }
    public Contractor? Contractor { get; set; }

    public Goal? Goal { get; init; }

    public TimeSpan TotalTime() {
        return GoalTime.IsNullOrEmpty() ? TimeSpan.Zero : GoalTime
        .Select(x => x.Time())
        .Aggregate((acc, time) => acc + time);
    }

    public override string ToString() => Name;
}
