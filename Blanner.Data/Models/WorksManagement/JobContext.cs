using Blanner.Data.Models.TimeRanges;

namespace Blanner.Data.Models.WorksManagement;
public class JobContext
{
    public int Id { get; set; }
    public DateOnly Date { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Comment { get; set; } = string.Empty;

    public bool Saved { get; set; }

    public TimeSpan ElapsedTime { get; set; }
    public DateTimeOffset Start { get; set; }
    public DateTimeOffset End { get; set; }

    public List<JobTime> Time { get; set; } = [];
    public List<JobChanges> Changes { get; set; } = [];
    
    public string? UserId { get; set; } = null!;
    public int? ContractorId { get; set; } = null!;

    public User? User { get; set; }
    public Contractor? Contractor { get; set; }

    public override string ToString() => Name;
}
