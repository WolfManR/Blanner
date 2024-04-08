namespace Blanner.Data.Models;
public class JobContext {
    public int Id { get; set; }
    public User? User { get; set; }
    public Contractor? Contractor { get; set; }
    public DateOnly Date { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Comment { get; set; } = string.Empty;

    public bool Saved { get; set; }

    public TimeSpan ElapsedTime { get; set; }
    public DateTimeOffset Start { get; set; }
    public DateTimeOffset End { get; set; }
    public List<JobTime> Time { get; set; } = [];

    public override string ToString() => Name;
}
