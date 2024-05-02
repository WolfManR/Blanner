using Blanner.Data.Models;

using Microsoft.EntityFrameworkCore;

namespace Blanner.Data; 
public class GoalsRepository(ApplicationDbContext dbContext) {
	private readonly ApplicationDbContext _dbContext = dbContext;

	public IQueryable<Goal> Goals(string userId, bool insertActive) {
		var query = _dbContext.Goals.AsNoTracking().Include(x => x.Contractor).AsQueryable();
		query = query.Include(x => x.User).Where(x => x.User != null && x.User.Id == userId);
		query = query.Include(x => x.ActiveGoal).Where(x => x.ActiveGoal != null == insertActive);
		return query;
	}

	public Task<Goal> Goal(int goalId) {
		return _dbContext.Goals.AsNoTracking().Include(x => x.User).Include(x => x.Contractor).Include(x => x.ActiveGoal).FirstAsync(x => x.Id == goalId);
	}

	public async ValueTask<Goal?> Save(string userId, string name) {
		User? user = await _dbContext.Users.FindAsync(userId);
		if (user is null) return null;
	
		Goal goal = new(name, user);
	
		_dbContext.Goals.Add(goal);
		await _dbContext.SaveChangesAsync();
	
		return goal;
	}

	public async Task<Goal> Update(int goalId, string name, int? contractorId) {
		Goal goal = await _dbContext.Goals
			.Include(x => x.Contractor)
			.Include(x => x.ActiveGoal).ThenInclude(x => x!.Contractor)
			.FirstAsync(x => x.Id == goalId);

		goal.Name = name;
		if (goal.ActiveGoal is not null) goal.ActiveGoal.Name = name;

		switch (goal.Contractor is null, contractorId is null) {
			case (false, true):
				goal.Contractor = null;
				if (goal.ActiveGoal is not null) goal.ActiveGoal.Contractor = null;
				break;
			case (_, false) when (goal.Contractor?.Id != contractorId):
				Contractor? contractor = await _dbContext.Contractors.FindAsync(contractorId);
				if (contractor is null) break;
				goal.Contractor = contractor;
				if (goal.ActiveGoal is not null) goal.ActiveGoal.Contractor = contractor;
				break;
			default:
				break;
		}

		await _dbContext.SaveChangesAsync();

		return goal;
	}

	public async Task<Goal?> Delete(int goalId) {
		Goal? goal = await _dbContext.Goals
			.Include(x => x.ActiveGoal).ThenInclude(x => x!.Tasks)
			.Include(x => x.Tasks)
			.FirstOrDefaultAsync(x => x.Id == goalId);
		if(goal is null) return null;

		await using var transaction = await _dbContext.Database.BeginTransactionAsync();

		List<ToDo> tasks = goal.Tasks;

		if(goal.ActiveGoal is not null) {
			tasks = [.. tasks, .. goal.ActiveGoal.Tasks];
		}

		tasks = tasks.DistinctBy(x => x.Id).ToList();
		_dbContext.RemoveRange(tasks);
		_dbContext.Remove(goal);

		await _dbContext.SaveChangesAsync();

		await transaction.CommitAsync();

		return goal;
	}

	public async Task<ActiveGoal> Activate(int goalId, DateTimeOffset activateDate) {
		ActiveGoal? activeGoal;
		activeGoal = await _dbContext.ActiveGoals
			.Include(x => x.Tasks)
			.Include(x => x.GoalTime)
			.Include(x => x.Contractor)
			.Include(x => x.Goal)
			.Include(x => x.User)
			.FirstOrDefaultAsync(x => x.Goal != null && x.Goal.Id == goalId);

		if (activeGoal is not null) return activeGoal;

		Goal goal = await _dbContext.Goals
			.Include(x => x.ActiveGoal)
			.Include(x => x.Contractor)
			.Include(x => x.Tasks)
			.Include(x => x.User)
			.FirstAsync(x => x.Id == goalId);

		activeGoal = new(goal, activateDate);
		goal.ActiveGoal = activeGoal;
		_dbContext.Add(activeGoal);

		await _dbContext.SaveChangesAsync();

		return activeGoal;
	}
}
