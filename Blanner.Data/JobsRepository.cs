using Blanner.Data.Models;

using Microsoft.EntityFrameworkCore;

namespace Blanner.Data;

public class JobsRepository(ApplicationDbContext dbContext) {
    private readonly ApplicationDbContext _dbContext = dbContext;

    public async Task<List<JobHeaderData>> List(string userId, DateTimeOffset start, DateTimeOffset end) {
        var data = await _dbContext.Jobs.AsNoTracking()
                 .Include(x => x.User)
                 .Where(x => x.User != null && x.User.Id == userId)
                 .Include(x => x.Time)
                 .Include(x => x.Contractor)
                 .Where(x => x.End > start && x.Start < end)
                 .Select(x => new JobHeaderData {
					 Id = x.Id,
					 User = x.User,
					 Contractor = x.Contractor,
					 Date = x.Date,
					 Name = x.Name,
					 Comment = x.Comment,
					 Saved = x.Saved,
					 ElapsedTime = x.ElapsedTime,
					 Start = x.Start,
					 End = x.End,
				 }).ToListAsync();

        return data;
    }

    public async Task<JobDetailsData?> Details(int id) {
        JobDetailsData? result = await _dbContext.Jobs
        .AsNoTracking()
		.Where(x => x.Id == id)
		.Include(x => x.User)
		.Include(x => x.Contractor)
		.Select(x => new JobDetailsData {
            Id = x.Id,
            User = x.User,
            Contractor = x.Contractor,
			Date = x.Date,
			Name = x.Name,
            Comment = x.Comment,
            Saved = x.Saved,
            Start = x.Start,
            End = x.End,
            Time = _dbContext.JobsTime.AsNoTracking().Where(t => t.Context.Id == id).Select(t => new JobDetailsTimeData {
				Id = t.Id,
				Start = t.Start,
				End = t.End
			}).ToList()
        })
        .FirstOrDefaultAsync();

        return result;
    }

    public async Task<bool> BuildJob(BuildJobData data) {
		User? user = await _dbContext.Users.FindAsync(data.UserId);
		if (user is null) return false;

		await using (var transaction = await _dbContext.Database.BeginTransactionAsync()) {
			var activeGoals = await _dbContext.ActiveGoals.AsNoTracking()
			.Include(x => x.User)
			.Where(x => x.User != null && x.User.Id == data.UserId && x.CurrentlyActiveTime != null)
			.Select(x => new { GoalId = x.Id, TimerId = x.CurrentlyActiveTime!.Value })
			.ToDictionaryAsync(x => x.GoalId, x => x.TimerId);

			var activeTimersId = activeGoals.Values.ToHashSet();
			var activeGoalsId = activeGoals.Keys.ToHashSet();

			await _dbContext.ActiveGoalsTime.Where(x => activeTimersId.Contains(x.Id)).ExecuteUpdateAsync(setters => setters.SetProperty(x => x.End, data.BuildDate));
			await _dbContext.ActiveGoals.Where(x => activeGoalsId.Contains(x.Id)).ExecuteUpdateAsync(setters => setters.SetProperty(x => x.CurrentlyActiveTime, x => null));

			await transaction.CommitAsync();
		}

		var goals = await _dbContext.ActiveGoals
			.Include(x => x.User).Where(x => x.User != null && x.User.Id == data.UserId)
			.Include(x => x.Contractor)
			.Include(x => x.Tasks.Where(x => x.Done))
			.Include(x => x.GoalTime)
			.AsSplitQuery()
			.ToListAsync();

		foreach (var goal in goals) {
            if (goal.GoalTime.Count <= 0) continue;
			var goalStart = goal.GoalTime.Select(x => x.Start).DefaultIfEmpty().Min();
            DateOnly goalDate = DateOnly.FromDateTime(goalStart.DateTime);
            string goalName = goal.Name.Trim();

            var context = await _dbContext.Jobs
				.Include(x => x.User)
                .Where(x => x.User != null && x.User.Id == data.UserId && x.Date == goalDate && x.Name == goalName)
                .Include(x => x.Contractor)
				.Include(x => x.Time)
				.AsSplitQuery()
				.FirstOrDefaultAsync();

            var goalTime = goal.GoalTime.Select(x => new JobTime() {
				User = user,
				Start = x.Start,
				End = x.End
			}).ToList();

            if(context is not null) {
                context.Time.AddRange(goalTime);

                context.Start = context.Time.Select(x => x.Start).DefaultIfEmpty().Max();
                context.End = context.Time.Select(x => x.End).DefaultIfEmpty().Max();
                context.Comment += $"{(context.Comment.Length > 0 ? "\n\n" : "")}{goal.Comment}";

                context.ElapsedTime = context.Time.Aggregate(TimeSpan.Zero, (acc, time) => acc + (time.End - time.Start));
            }
            else {
				var goalEnd = goal.GoalTime.Select(x => x.Start).DefaultIfEmpty().Max();
				var goalElapsedTime = goal.TotalTime();

				context ??= new() {
					User = user,
					Contractor = goal.Contractor,
					Name = goalName,
					Comment = goal.Comment,
					Date = goalDate,
					Start = goalStart,
					End = goalEnd,
					ElapsedTime = goalElapsedTime,
                    Time = goalTime
				};
			}

			_dbContext.Jobs.Update(context);
		}

		await _dbContext.SaveChangesAsync();

		await using (var transaction = await _dbContext.Database.BeginTransactionAsync()) {
			await _dbContext.ActiveGoals
				.Include(x => x.User)
                .Where(x => x.User != null && x.User.Id == data.UserId)
                .Include(x => x.GoalTime)
				.ExecuteDeleteAsync();

			await _dbContext.ToDos
				.Include(x => x.User)
				.Where(x => x.User != null && x.User.Id == data.UserId)
				.Include(x => x.Goal)
				.Include(x => x.ActiveGoal)
				.Where(x => x.Done && x.Goal != null && x.ActiveGoal != null)
				.ExecuteDeleteAsync();

			await transaction.CommitAsync();
		}

		return true;
	}

	public async Task<JobContext> Update(JobHeaderSaveData data) {
		JobContext context = await _dbContext.Jobs
			   .Where(x => x.Id == data.JobId)
			   .Include(x => x.User)
			   .Include(x => x.Contractor)
			   .AsSplitQuery()
			   .FirstAsync();

		context.Name = data.Name;
		context.Comment = data.Comment;

		switch (context.Contractor is null, data.ContractorId is null) {
			case (false, true):
				context.Contractor = null;
				break;
			case (_, false) when (context.Contractor?.Id != data.ContractorId):
				Contractor? contractor = await _dbContext.Contractors.FindAsync(data.ContractorId);
				if (contractor is null) break;
				context.Contractor = contractor;
				break;
			default:
				break;
		}

		await _dbContext.SaveChangesAsync();

		return context;
	}

	public async Task<JobContext> Update(JobSavedChangedData data) {
		JobContext context = await _dbContext.Jobs
			   .Where(x => x.Id == data.JobId)
			   .FirstAsync();

		context.Saved = data.Saved;

		await _dbContext.SaveChangesAsync();

		return context;
	}
}
