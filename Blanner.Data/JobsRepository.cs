using Blanner.Data.Models;
using Blanner.Data.Models.TimeRanges;
using Microsoft.EntityFrameworkCore;

namespace Blanner.Data;

public class JobsRepository(ApplicationDbContext dbContext) {
    private readonly ApplicationDbContext _dbContext = dbContext;

    public async Task<List<JobHeaderData>> List(string userId, DateOnly start, DateOnly end) {
        var data = await _dbContext.Jobs.AsNoTracking()
                 .Include(x => x.User)
                 .Where(x => x.User != null && x.User.Id == userId)
                 .Where(x => x.Date > start && x.Date < end)
				 .Include(x => x.Changes)
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
					 Changes = x.Changes
						.Select(ch => new JobChangesData() {
							Id = ch.Id,
							Comment = ch.Comment,
							Saved = ch.Saved,
							Start = ch.Start,
							End = ch.End,
							ElapsedTime = ch.ElapsedTime
						})
						.ToList()
				 })
				 .AsSplitQuery()
				 .ToListAsync();

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
			var activeGoals = await _dbContext.Goals.AsNoTracking()
			.Include(x => x.User)
			.Where(x => x.User != null && x.User.Id == data.UserId && x.CurrentlyActiveTime != null)
			.Select(x => new { GoalId = x.Id, TimerId = x.CurrentlyActiveTime!.Value })
			.ToDictionaryAsync(x => x.GoalId, x => x.TimerId);

			var activeTimersId = activeGoals.Values.ToHashSet();
			var activeGoalsId = activeGoals.Keys.ToHashSet();

			await _dbContext.ActiveGoalsTime.Where(x => activeTimersId.Contains(x.Id)).ExecuteUpdateAsync(setters => setters.SetProperty(x => x.End, data.BuildDate));
			await _dbContext.Goals.Where(x => activeGoalsId.Contains(x.Id)).ExecuteUpdateAsync(setters => setters.SetProperty(x => x.CurrentlyActiveTime, x => null));

			await transaction.CommitAsync();
		}

		var goals = await _dbContext.Goals
			.Include(x => x.User).Where(x => x.User != null && x.User.Id == data.UserId)
			.Include(x => x.Contractor)
			.Include(x => x.Tasks.Where(x => x.Done))
			.Include(x => x.GoalTime)
			.AsSplitQuery()
			.ToListAsync();

		foreach (var goal in goals) {
            if (goal.GoalTime.Count <= 0 && string.IsNullOrWhiteSpace(goal.Comment)) continue;

			var goalTime = goal.GoalTime.Select(x => new JobTime() {
				User = user,
				Start = x.Start,
				End = x.End
			}).ToList();

			var goalStart = goalTime.Select(x => x.Start).DefaultIfEmpty().Min();
			var goalEnd = goalTime.Select(x => x.Start).DefaultIfEmpty().Max();
			var goalElapsedTime = goal.TotalTime();
			DateOnly goalDate = DateOnly.FromDateTime(goalStart.DateTime);
			if (goalDate == DateOnly.MinValue) goalDate = DateOnly.FromDateTime(DateTime.Now);
            string goalName = goal.Name.Trim();
			Contractor? goalContractor = goal.Contractor;
			
			var context = await _dbContext.Jobs
				.Include(x => x.User)
                .Where(x => x.User != null && x.User.Id == data.UserId && x.Date == goalDate && x.Name == goalName)
                .Include(x => x.Contractor)
				.Where(x => (x.Contractor == null && goalContractor == null) || (x.Contractor != null && goalContractor != null && x.Contractor.Id == goalContractor.Id))
				.Include(x => x.Time)
				.AsSplitQuery()
				.FirstOrDefaultAsync();

            if(context is not null) {
				if(context.Changes.Count == 0) {
					context.Changes.Add(new() {
						Comment = context.Comment,
						Start = context.Start,
						End = context.End,
						ElapsedTime = context.ElapsedTime,
						Time = [.. context.Time]
					});
				}

                context.Time.AddRange(goalTime);

                context.Start = context.Time.Select(x => x.Start).DefaultIfEmpty().Min();
                context.End = context.Time.Select(x => x.End).DefaultIfEmpty().Max();
                context.Comment += $"{(context.Comment.Length > 0 ? "\n\n" : "")}{goal.Comment}";

                context.ElapsedTime = context.Time.Aggregate(TimeSpan.Zero, (acc, time) => acc + (time.End - time.Start));

				context.Saved = false;

				context.Changes.Add(new() {
					Comment = goal.Comment,
					Start = goalStart,
					End = goalEnd,
					ElapsedTime = goalElapsedTime,
					Time = goalTime
				});
            }
            else {
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
			await _dbContext.Goals
				.Include(x => x.User)
                .Where(x => x.User != null && x.User.Id == data.UserId)
                .Include(x => x.GoalTime)
				.ExecuteDeleteAsync();

			await _dbContext.ToDos
				.Include(x => x.User)
				.Where(x => x.User != null && x.User.Id == data.UserId)
				.Include(x => x.Goal)
				.Where(x => x.Done && x.Goal != null)
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
			   .Include(x => x.Changes.Where(ch => !ch.Saved))
			   .FirstAsync();

		context.Saved = data.Saved;
        foreach (var change in context.Changes)
        {
			change.Saved = true;
        }

        await _dbContext.SaveChangesAsync();

		return context;
	}
}
