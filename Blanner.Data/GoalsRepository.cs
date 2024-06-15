using Blanner.Data.DataModels;
using Blanner.Data.Models;

using Microsoft.EntityFrameworkCore;

namespace Blanner.Data; 
public class GoalsRepository(ApplicationDbContext dbContext) {
	private readonly ApplicationDbContext _dbContext = dbContext;

	public IQueryable<Goal> Goals(string userId) {
		var query = _dbContext.Goals.AsNoTracking().Include(x => x.Contractor).Include(x => x.GoalTime).AsQueryable();
		query = query.Include(x => x.User).Where(x => x.User != null && x.User.Id == userId);
		return query;
	}

	public Task<Goal> Goal(int goalId) {
		return _dbContext.Goals.AsNoTracking()
			.Include(x => x.Contractor)
			.Include(x => x.GoalTime)
			.Include(x => x.Tasks)
			.Include(x => x.User)
			.FirstAsync(x => x.Id == goalId);
	}

	public Task<List<ActiveTimerData>> ActiveTimers() {
		return _dbContext.Goals
			.Include(x => x.User)
			.Where(x => x.User != null && x.CurrentlyActiveTime != null)
			.Select(x=> new ActiveTimerData(x.Id, x.CurrentlyActiveTime!.Value, x.User!.Id))
			.ToListAsync();
	}

	public async Task<TimeSpan> TotalTime(int goalId) {
		var goalTime = await _dbContext.ActiveGoalsTime.Include(x => x.ActiveGoal).Where(x => x.ActiveGoal.Id == goalId).Select(x => x.End - x.Start).ToListAsync();

		return goalTime?.Count > 0 ? goalTime.Aggregate((cum, t) => cum + t) : TimeSpan.Zero;
	}

	public async Task<Goal> Create(GoalCreationData data) {
		User? user = await _dbContext.Users.FindAsync(data.UserId) ?? throw new InvalidOperationException("User not existed");
		
		Goal goal = new() { Name = data.Name, User = user, Comment = data.Comment };
		if (await _dbContext.Contractors.FindAsync(data.ContractorId) is { } contractor) goal.Contractor = contractor;
		
		_dbContext.Goals.Add(goal);
		await _dbContext.SaveChangesAsync();

		return goal;
	}

	public async Task<Goal> Update(GoalHeaderDataChanges data) {
		Goal goal = await _dbContext.Goals
			.Include(x => x.Contractor)
			.FirstAsync(x => x.Id == data.Id);

		goal.Name = data.Name;
		goal.Comment = data.Comment;

		switch (goal.Contractor is null, data.ContractorId is null) {
			case (false, true):
				goal.Contractor = null;
				break;
			case (_, false) when (goal.Contractor?.Id != data.ContractorId):
				Contractor? contractor = await _dbContext.Contractors.FindAsync(data.ContractorId);
				if (contractor is null) break;
				goal.Contractor = contractor;
				break;
			default:
				break;
		}

		await _dbContext.SaveChangesAsync();

		return goal;
	}

	public async Task<Goal?> Delete(int goalId) {
		Goal? goal = await _dbContext.Goals
			.Include(x => x.Tasks)
			.FirstOrDefaultAsync(x => x.Id == goalId);
		if (goal is null) return null;

		await using var transaction = await _dbContext.Database.BeginTransactionAsync();

		var tasks = goal.Tasks
			//.ExceptBy(goal.Goal is null ? [] : goal.Goal.Tasks.Select(x => x.Id), x => x.Id)
			.ToList();
		_dbContext.RemoveRange(tasks);
		_dbContext.Remove(goal);

		await _dbContext.SaveChangesAsync();

		await transaction.CommitAsync();

		return goal;
	}

	public async Task<ActiveGoalTime> StartTimer(int goalId, DateTimeOffset timeActivatinDate) {
		Goal goal = await _dbContext.Goals
			.Include(x => x.Contractor)
			.Include(x => x.Tasks)
			.Include(x => x.GoalTime)
			.Include(x => x.User)
			.FirstAsync(x => x.Id == goalId);

		var activeTime = await StartTimer(goal, timeActivatinDate);

		return activeTime;
	}

	public async Task<ActiveGoalTime> StartTimer(Goal goal, DateTimeOffset timeActivationDate) {
		ActiveGoalTime? activeTime = null;
	
		if(goal.CurrentlyActiveTime is not null) {
			activeTime = goal.GoalTime.Find(x => x.Id == goal.CurrentlyActiveTime);
			if (activeTime is not null) return activeTime;
		}

		activeTime = new() { Start = timeActivationDate, ActiveGoal = goal };
		goal.GoalTime.Add(activeTime);

		await _dbContext.SaveChangesAsync();

		goal.CurrentlyActiveTime = activeTime.Id;

		await _dbContext.SaveChangesAsync();

		return activeTime;
	}

	public async Task<TimeSpan> StopTimer(int goalId, DateTimeOffset timeStopDate) {
		Goal goal = await _dbContext.Goals
			.Include(x => x.GoalTime)
			.FirstAsync(x => x.Id == goalId);
		if (goal.CurrentlyActiveTime is null) throw new InvalidOperationException("Timer already stopped");

		ActiveGoalTime? activeTime = goal.GoalTime.Find(x => x.Id == goal.CurrentlyActiveTime) ?? throw new InvalidOperationException("Timer set but not exist");
		activeTime.End = timeStopDate;
		goal.CurrentlyActiveTime = null;

		await _dbContext.SaveChangesAsync();

		return goal.TotalTime();
	}


	public async Task<(ActiveGoalTime, TimeSpan)?> CreateTimer(GoalTimeCreationData data) {
		if (await _dbContext.Goals.AllAsync(x => x.Id != data.GoalId)) return null;
		DateTimeOffset StartTime = data.Start;
		DateTimeOffset EndTime = DateTimeOffset.MinValue;

		if (data.End.HasValue) {
			EndTime = data.End.Value;
		}
		else if (data.Time.HasValue) {
			EndTime = StartTime + data.Time.Value;
		}
		else {
			return null;
		}
		if (EndTime < StartTime) return null;

        Goal goal = await _dbContext.Goals.Include(x=>x.GoalTime).FirstAsync(x => x.Id == data.GoalId);

		ActiveGoalTime goalTime = new() {
			ActiveGoal = goal,
			Start = StartTime,
			End = EndTime
		};

		goal.GoalTime.Add(goalTime);

		await _dbContext.SaveChangesAsync();

		return (goalTime, goal.TotalTime());
	}

	public async Task<(ActiveGoalTime, TimeSpan)?> EditTimer(GoalTimeEditData data) {
		if (await _dbContext.Goals.AllAsync(x => x.Id != data.GoalId)) return null;
		DateTimeOffset StartTime = data.Start;
		DateTimeOffset EndTime = DateTimeOffset.MinValue;

		if (data.End.HasValue) {
			EndTime = data.End.Value;
		}
		else if (data.Time.HasValue) {
			EndTime = StartTime + data.Time.Value;
		}
		else {
			return null;
		}
		if (EndTime < StartTime) return null;

		ActiveGoalTime goalTime = await _dbContext.ActiveGoalsTime
		.Include(x => x.ActiveGoal).ThenInclude(x => x.GoalTime)
		.Where(x => x.ActiveGoal.Id == data.GoalId)
		.FirstAsync(x => x.Id == data.TimerId);

		goalTime.Start = StartTime;
		goalTime.End = EndTime;

		await _dbContext.SaveChangesAsync();

		return (goalTime, goalTime.ActiveGoal.TotalTime());
	}

	public async Task<ActiveGoalTime?> DeleteTimer(int timeId) {
		ActiveGoalTime? goalTime = await _dbContext.ActiveGoalsTime
			.Include(x => x.ActiveGoal).ThenInclude(x => x.GoalTime)
			.FirstOrDefaultAsync(x=> x.Id == timeId);
		if (goalTime is null) return null;

		goalTime.ActiveGoal.GoalTime.Remove(goalTime);
		
		await _dbContext.SaveChangesAsync();

		return goalTime;
	}

	public async Task<List<Goal>> CompleteJob(string userId, DateTimeOffset endTime) {
		var activeGoals = await _dbContext.Goals
			.Include(x => x.User)
			.Where(x => x.User != null && x.User.Id == userId && x.CurrentlyActiveTime != null)
			.Select(x => new { x.Id, TimerId = x.CurrentlyActiveTime!.Value })
			.ToListAsync();

		var goalsId = activeGoals.Select(x => x.Id).ToHashSet();
		var timersId = activeGoals.Select(x => x.TimerId).ToHashSet();

		using (var transaction = await _dbContext.Database.BeginTransactionAsync()) {
			await _dbContext.ActiveGoalsTime.Where(x => timersId.Contains(x.Id)).ExecuteUpdateAsync(setters => setters.SetProperty(x => x.End, endTime));
			await _dbContext.Goals.Where(x => goalsId.Contains(x.Id)).ExecuteUpdateAsync(setters => setters.SetProperty(x => x.CurrentlyActiveTime, x => null));
			await transaction.CommitAsync();
		}

		List<Goal> goals = await _dbContext.Goals.AsNoTracking().Include(x => x.GoalTime).Where(x => goalsId.Contains(x.Id)).ToListAsync();

		return goals;
	}

	public async Task<GoalTemplate?> SaveAsTemplate(GoalSaveAsTemplateData request) {
		var goal = await _dbContext.Goals.Where(x => x.Id == request.GoalId).Select(x => new {x.User, x.ContractorId, x.Contractor, x.Name, x.Comment }).FirstOrDefaultAsync();
        if (goal is null) return null;
		var templateExist = await _dbContext.GoalsTemplates.Where(x => x.UserId == request.UserId && x.ContractorId == goal.ContractorId && x.Name == goal.Name && x.Comment == goal.Comment).AnyAsync();
		if (templateExist) return null;
		
		GoalTemplate template = new() {
			Name = goal.Name,
			Comment = goal.Comment,
			Contractor = goal.Contractor,
			CreatedAt = DateTimeOffset.Now,
			User = goal.User
		};

		_dbContext.Update(template);

		await _dbContext.SaveChangesAsync();

		return template;
	}
}
