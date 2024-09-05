using Blanner.Data.Models.TimeRanges;

namespace Blanner.Data.Models.WorksManagement;
public class Goal() : BaseModel
{
    public Goal(GoalTemplate template, DateTimeOffset createDate) : this()
    {
        CreatedAt = createDate;
        Name = template.Name;
        User = template.User;
        Comment = template.Comment;
    }
    
    public Goal(GoalTemplate template, Contractor? contractor, DateTimeOffset createDate) : this(template, createDate)
    {
        Contractor = contractor;
    }

    public string Name { get; set; } = string.Empty;
    public string Comment { get; set; } = string.Empty;

    public List<ToDo> Tasks { get; set; } = [];
    public List<ActiveGoalTime> GoalTime { get; set; } = [];
    public int? CurrentlyActiveTime { get; set; }
    public int? GoalGroupId { get; set; }

    public string UserId { get; set; } = null!;
	public int? ContractorId { get; set; }
	public User? User { get; set; }
    public Contractor? Contractor { get; set; }
    public GoalsGroup? Group { get; set; }

    public TimeSpan TotalTime()
    {
        return GoalTime.IsNullOrEmpty() ? TimeSpan.Zero : GoalTime
        .Select(x => x.Time())
        .Aggregate((acc, time) => acc + time);
    }

    public override string ToString() => Name;
}
