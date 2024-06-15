namespace Blanner.Data.Models;
public class ToDo {
    public long Id { get; set; }
    public bool Done { get; set; }
    public string? Name { get; set; }

    public Goal? Goal { get; set; }
    public User? User { get; set; }
}
