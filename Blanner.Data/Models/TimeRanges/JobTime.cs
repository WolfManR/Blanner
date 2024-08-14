using Blanner.Data.DataModels;
using Blanner.Data.Models.WorksManagement;

namespace Blanner.Data.Models.TimeRanges;
public class JobTime : TimeRange, ITime {
    public long Id { get; set; }

    public string? UserId { get; set; }
    public int? ContextId { get; set; }
    public int? JobChangeId { get; set; }

    public User? User { get; set; }
    public JobContext Context { get; set; } = default!;
    public JobChanges? JobChange { get; set; }
}
