using Blanner.Data.Models;

using System.Text.Json.Serialization;

namespace Blanner.Data;

public class GoalData {
	[JsonConstructor]
	public GoalData() { }

	public GoalData(Goal data) {
		Id = data.Id;
		Name = data.Name;
		Contractor = data.Contractor;
		ActiveGoalId = data.ActiveGoal?.Id;
		Tasks = data.Tasks;
	}

	public int Id { get; set; }
	public string Name { get; set; } = string.Empty;
	public Contractor? Contractor { get; set; }
	public int? ActiveGoalId { get; set; }
	public List<ToDo> Tasks { get; set; } = [];
}

public class GoalDetailsData {
	[JsonConstructor]
	public GoalDetailsData() { }

	public GoalDetailsData(Goal data) {
		Id = data.Id;
		Name = data.Name;
		Contractor = data.Contractor;
		ActiveGoalId = data.ActiveGoal?.Id;
		User = data.User;
		Tasks = data.Tasks;
	}

	public int Id { get; set; }
	public string Name { get; set; } = string.Empty;
	public Contractor? Contractor { get; set; }
	public int? ActiveGoalId { get; set; }
	public User? User { get; set; }
	public List<ToDo> Tasks { get; set; } = [];
}

public class ActiveGoalData {
	[JsonConstructor]
	public ActiveGoalData() { }

	public ActiveGoalData(ActiveGoal data) {
		Id = data.Id;
		Name = data.Name;
		Comment = data.Comment;
		Contractor = data.Contractor;
		GoalId = data.Goal.Id;
		Tasks = data.Tasks;
		TotalElapsedTime = data.TotalTime();
		ActivationTime = data.CurrentlyActiveTime == null ? DateTimeOffset.MinValue : data.GoalTime.Find(c => c.Id == data.CurrentlyActiveTime)!.Start;
		ActiveTimerId = data.CurrentlyActiveTime;
	}

	public int Id { get; set; }
	public string Name { get; set; } = string.Empty;
	public string Comment { get; set; } = string.Empty;
	public Contractor? Contractor { get; set; }
	public int GoalId { get; set; }
	public List<ToDo> Tasks { get; set; } = [];
	public TimeSpan TotalElapsedTime { get; set; }
	public DateTimeOffset ActivationTime { get; set; }
	public int? ActiveTimerId { get; set; }
}
public class ActiveGoalDetailsData {
	[JsonConstructor]
	public ActiveGoalDetailsData() { }

	public ActiveGoalDetailsData(ActiveGoal data) {
		Id = data.Id;
		Name = data.Name;
		Comment = data.Comment;
		Contractor = data.Contractor;
		User = data.User;
		GoalId = data.Goal.Id;
		Tasks = data.Tasks;
		GoalTime = data.GoalTime.Select(x => new ActiveGoalTimeData(x)).ToList();
		ActiveTimerId = data.CurrentlyActiveTime;
	}

	public int Id { get; set; }
	public string Name { get; set; } = string.Empty;
	public string Comment { get; set; } = string.Empty;
	public Contractor? Contractor { get; set; }
	public User? User { get; set; }
	public int GoalId { get; set; }
	public List<ToDo> Tasks { get; set; } = [];
	public List<ActiveGoalTimeData> GoalTime { get; set; } = [];
	public int? ActiveTimerId { get; set; }
}


public class ActiveGoalTimeData : IComparable<ActiveGoalTimeData> {
	[JsonConstructor]
	public ActiveGoalTimeData() {

	}

	public ActiveGoalTimeData(ActiveGoalTime data) {
		Id = data.Id;
		Start = data.Start;
		End = data.End;
		Time = data.Time();
	}

	public int Id { get; set; }
	public DateTimeOffset Start { get; set; }
	public DateTimeOffset End { get; set; }
	public TimeSpan Time { get; set; }

	public int CompareTo(ActiveGoalTimeData? other) {
		if (other is null) return 1;
		int compareResult = Start.CompareTo(other.Start);
		if (compareResult < 0) return -1;
		else if (compareResult > 0) return 1;
		return End.CompareTo(other.End);
	}
}

public class JobHeaderData {
	public int Id { get; set; }
	public DateTimeOffset Start { get; set; }
	public DateTimeOffset End { get; set; }

	public Contractor? Contractor { get; set; }
	public string Name { get; set; } = string.Empty;
	public string Comment { get; set; } = string.Empty;

	public bool Marked { get; set; }
	public string? MarkComment { get; set; }

	public TimeSpan TotalTime { get; set; }
}

public class JobDetailsData {
	public int Id { get; set; }
	public DateTimeOffset Start { get; set; }
	public DateTimeOffset End { get; set; }

	public Contractor? Contractor { get; set; }
	public User? User { get; set; }
	public string Name { get; set; } = string.Empty;
	public string Comment { get; set; } = string.Empty;

	public bool Marked { get; set; }
	public string? MarkComment { get; set; }

	public List<JobDetailsTimeData> Time { get; set; } = [];
}

public class JobDetailsTimeData {
	public long Id { get; set; }
	public DateTimeOffset Start { get; set; }
	public DateTimeOffset End { get; set; }
}

public class UserSelectListData {
	[JsonConstructor]
	public UserSelectListData()
    {
        
    }
    public UserSelectListData(User data)
    {
		Id = data.Id;
		Name = data.UserName;
    }

    public string Id { get; set; } = default!;
	public string? Name { get; set; }
}

public class UserInfoData {
	[JsonConstructor]
	public UserInfoData()
    {
        
    }
    public UserInfoData(User data) {
		Id = data.Id;
		Name = data.UserName;
	}

	public string Id { get; set; } = default!;
	public string? Name { get; set; }
}