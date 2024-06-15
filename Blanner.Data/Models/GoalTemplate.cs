namespace Blanner.Data.Models; 
public class GoalTemplate() : BaseModel
{
    public GoalTemplate(string name, User user) : this() {
        Name = name;
        User = user;
    }

    public string Name { get; set; } = string.Empty;
    public string Comment { get; set; } = string.Empty;

    public string UserId { get; set; } = null!;
    public User? User { get; set; }
    public int? ContractorId { get; set; }
    public Contractor? Contractor { get; set; }

    public override string ToString() => Name;
}
