namespace Blanner.Data.Models; 
public class Goal() : BaseModel
{
    public Goal(GoalTemplate template, DateTimeOffset createDate) : this()
    {
        CreatedAt = createDate;
        Name = template.Name;
        Contractor = template.Contractor;
        User = template.User;
        Comment = template.Comment;
    }

    public string Name { get; set; } = string.Empty;
    public string Comment { get; set; } = string.Empty;

    public List<ToDo> Tasks { get; set; } = [];
    public List<ActiveGoalTime> GoalTime { get; set; } = [];
    public int? CurrentlyActiveTime { get; set; }

    public string UserId { get; set; } = null!;
    public User? User { get; set; }
    public int? ContractorId { get; set; }
    public Contractor? Contractor { get; set; }

    public TimeSpan TotalTime() {
        return GoalTime.IsNullOrEmpty() ? TimeSpan.Zero : GoalTime
        .Select(x => x.Time())
        .Aggregate((acc, time) => acc + time);
    }

    public override string ToString() => Name;
}
