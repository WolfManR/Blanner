namespace Blanner.Data.Models.WorksManagement;
public class GoalTemplate() : BaseModel
{
    public GoalTemplate(string name, User user) : this()
    {
        Name = name;
        User = user;
    }

    public string Name { get; set; } = string.Empty;
    public string Comment { get; set; } = string.Empty;

    public List<Contractor> Contractors { get; set; } = [];

    public string UserId { get; set; } = null!;
    public int? ContractorId { get; set; }
	public User? User { get; set; }
    public Contractor? Contractor { get; set; }

    public override string ToString() => Name;
}
