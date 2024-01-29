namespace Blanner.Data.Models {
	public class Goal() : BaseModel
	{
	    public Goal(string name, User user) : this() {
	        Name = name;
	        User = user;
	    }

	    public string Name { get; set; } = string.Empty;
	    public List<ToDo> Tasks { get; set; } = [];

	    public User? User { get; set; }
	    public Contractor? Contractor { get; set; }
	    public ActiveGoal? ActiveGoal { get; set; }
	}
}
