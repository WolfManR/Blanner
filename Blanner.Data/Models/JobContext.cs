namespace Blanner.Data.Models;
public class JobContext {
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Comment { get; set; } = string.Empty;
    public bool Marked { get; set; }
    public string? MarkComment { get; set; }
    public Contractor? Contractor { get; set; }
}
