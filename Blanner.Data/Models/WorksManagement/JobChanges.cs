using Blanner.Data.Models.TimeRanges;

namespace Blanner.Data.Models.WorksManagement;
public class JobChanges
{
    public int Id { get; set; }

    public string Comment { get; set; } = string.Empty;
    public bool Saved { get; set; }

    public TimeSpan ElapsedTime { get; set; }
    public DateTimeOffset Start { get; set; }
    public DateTimeOffset End { get; set; }

    public List<JobTime> Time { get; set; } = [];

    public int? ContextId { get; set; }
    public JobContext? Context { get; set; }

    public override string ToString() => Comment;
}
