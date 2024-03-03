namespace Blanner.Data.Models; 
public class Contractor() : BaseModel
{
    public Contractor(string name) : this() => Name = name;

    public string Name { get; set; } = string.Empty;

    public override string ToString() => Name;
}
