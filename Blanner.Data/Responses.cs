using Blanner.Data.DataModels;
using Blanner.Data.Models;
using Blanner.Data.Models.TimeRanges;
using System.Text.Json.Serialization;

namespace Blanner.Data;

public class GoalMainData {
	[JsonConstructor]
	public GoalMainData() { }

	public GoalMainData(GoalTemplate data) {
		Id = data.Id;
		Name = data.Name;
		Comment = data.Comment;
		Contractor = data.Contractor;
		User = data.User is not null ? new(data.User) : null;
	}

	public int Id { get; set; }
	public string Name { get; set; } = string.Empty;
	public string Comment { get; set; } = string.Empty;
	public Contractor? Contractor { get; set; }
	public UserInfoData? User { get; set; }
}

public class ActiveGoalListData : GoalMainData {
	[JsonConstructor]
	public ActiveGoalListData() { }

	public ActiveGoalListData(Goal data) {
		Id = data.Id;
		Name = data.Name;
		Comment = data.Comment;
		Contractor = data.Contractor;
		User = data.User is null ? null : new(data.User);

		TotalElapsedTime = data.TotalTime();
		ActivationTime = data.CurrentlyActiveTime == null ? DateTimeOffset.MinValue : data.GoalTime.Find(c => c.Id == data.CurrentlyActiveTime)!.Start;
		ActiveTimerId = data.CurrentlyActiveTime;

		Tasks = data.Tasks;
	}

	public TimeSpan TotalElapsedTime { get; set; }
	public DateTimeOffset ActivationTime { get; set; }
	public int? ActiveTimerId { get; set; }
	public List<ToDo> Tasks { get; set; } = [];
}
public class ActiveGoalDetailsData : GoalMainData {
	[JsonConstructor]
	public ActiveGoalDetailsData() { }

	public ActiveGoalDetailsData(Goal data) {
		Id = data.Id;
		Name = data.Name;
		Comment = data.Comment;
		Contractor = data.Contractor;
		User = data.User is null ? null : new(data.User);

		GoalTime = data.GoalTime.Select(x => new ActiveGoalTimeData(x)).ToList();
		ActiveTimerId = data.CurrentlyActiveTime;

		Tasks = data.Tasks;
	}

	public List<ActiveGoalTimeData> GoalTime { get; set; } = [];
	public int? ActiveTimerId { get; set; }

	public List<ToDo> Tasks { get; set; } = [];
}


public class ActiveGoalTimeData : IComparable<ActiveGoalTimeData>, ITime {
	[JsonConstructor]
	public ActiveGoalTimeData() {

	}

	public ActiveGoalTimeData(ActiveGoalTime data) {
		Id = data.Id;
		Start = data.Start;
		End = data.End;
	}

	public int Id { get; set; }
	public DateTimeOffset Start { get; set; }
	public DateTimeOffset End { get; set; }

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

	public User? User { get; set; }
	public Contractor? Contractor { get; set; }
	public DateOnly Date { get; set; }
	public string Name { get; set; } = string.Empty;
	public string Comment { get; set; } = string.Empty;

	public bool Saved { get; set; }

	public TimeSpan ElapsedTime { get; set; }
	public DateTimeOffset Start { get; set; }
	public DateTimeOffset End { get; set; }

	public List<JobChangesData> Changes { get; set; } = [];
}

public class JobChangesData {
	public int Id { get; set; }

	public string Comment { get; set; } = string.Empty;

	public bool Saved { get; set; }

	public TimeSpan ElapsedTime { get; set; }
	public DateTimeOffset Start { get; set; }
	public DateTimeOffset End { get; set; }
}

public class JobEditableHeaderData {
	public User? User { get; set; }
	public Contractor? Contractor { get; set; }
	public string Name { get; set; } = string.Empty;
	public string Comment { get; set; } = string.Empty;
}

public class JobDetailsData {
	public int Id { get; set; }

	public User? User { get; set; }
	public Contractor? Contractor { get; set; }
	public DateOnly Date { get; set; }
	public string Name { get; set; } = string.Empty;
	public string Comment { get; set; } = string.Empty;

	public bool Saved { get; set; }

	public DateTimeOffset Start { get; set; }
	public DateTimeOffset End { get; set; }

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

public class ActiveUserWorkData {
	public UserInfoData User { get; set; } = default!;
	public List<WorkData> WorkData { get; set; } = [];
}

public class WorkData {
	public int Id { get; set; }
	public string Name { get; set; } = string.Empty;
}