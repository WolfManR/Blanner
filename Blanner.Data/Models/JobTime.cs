using Blanner.Data.DataModels;

namespace Blanner.Data.Models;
public class JobTime : ITime
{
    public long Id { get; set; }

    public DateTimeOffset Start { get; set; }
    public DateTimeOffset End { get; set; }

    public User? User { get; set; }
    public JobContext Context { get; set; } = default!;
}
