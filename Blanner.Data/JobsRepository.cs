using Blanner.Data.Models;

using Microsoft.EntityFrameworkCore;

namespace Blanner.Data;

public class JobsRepository(ApplicationDbContext dbContext) {
    private readonly ApplicationDbContext _dbContext = dbContext;

    public async Task<List<JobHeaderData>> List(string userId, DateTimeOffset start, DateTimeOffset end) {
        var data = await _dbContext.JobsTime.AsNoTracking()
                 .Include(x => x.User)
                 .Where(x => x.User != null && x.User.Id == userId)
                 .Include(x => x.Context).ThenInclude(x => x.Contractor)
                 .Where(x => x.End > start && x.Start < end)
                 .Select(x => x.Context).Distinct()
                 .Select(x => new {
                     Context = x,
                     Start = _dbContext.JobsTime.Include(x => x.Context).Where(t => t.Context == x).Select(t => t.Start).Min(),
                     End = _dbContext.JobsTime.Include(x => x.Context).Where(t => t.Context == x).Select(t => t.End).Max(),
                     TotalTime = _dbContext.JobsTime.Include(x => x.Context).Where(t => t.Context == x).Select(t => t.End - t.Start).ToList()
                 }).ToListAsync();

        List<JobHeaderData> result = data
        .Select(x => new JobHeaderData {
            Id = x.Context.Id,
            Start = x.Start,
            End = x.End,
            Contractor = x.Context.Contractor,
            Name = x.Context.Name,
            Comment = x.Context.Comment,
            Marked = x.Context.Marked,
            MarkComment = x.Context.MarkComment,
            TotalTime = x.TotalTime.Aggregate((cum, t) => cum + t)
        }).ToList();

        return result;
    }

    public async Task<JobDetailsData?> Details(int id) {
        JobDetailsData? result = await _dbContext.JobsTime
        .AsNoTracking()
        .Include(x => x.Context).ThenInclude(x => x.Contractor)
        .Where(x => x.Context.Id == id)
        .Select(x => x.Context).Distinct()
        .Select(x => new {
            Context = x,
            User = _dbContext.JobsTime.Include(x => x.Context).Include(x => x.User).Where(t => t.Context == x).Select(x => x.User).First(),
            Start = _dbContext.JobsTime.Include(x => x.Context).Where(t => t.Context == x).Select(t => t.Start).Min(),
            End = _dbContext.JobsTime.Include(x => x.Context).Where(t => t.Context == x).Select(t => t.End).Max(),
            Time = _dbContext.JobsTime.Include(x => x.Context).Where(t => t.Context == x).Select(t => new JobDetailsTimeData {
                Id = t.Id,
                Start = t.Start,
                End = t.End
            }).ToList()
		})
        .Select(x => new JobDetailsData {
            Id = x.Context.Id,
            Start = x.Start,
            End = x.End,
            Contractor = x.Context.Contractor,
            User = x.User,
            Name = x.Context.Name,
            Comment = x.Context.Comment,
            Marked = x.Context.Marked,
            MarkComment = x.Context.MarkComment,
            Time = x.Time
        })
        .FirstOrDefaultAsync();

        return result;
    }

    public async Task<bool> BuildJob(BuildJobData data) {
		User? user = await dbContext.Users.FindAsync(data.UserId);
		if (user is null) return false;

		await using (var transaction = await dbContext.Database.BeginTransactionAsync()) {
			var activeGoals = await dbContext.ActiveGoals.AsNoTracking()
			.Include(x => x.User)
			.Where(x => x.User != null && x.User.Id == data.UserId && x.CurrentlyActiveTime != null)
			.Select(x => new { GoalId = x.Id, TimerId = x.CurrentlyActiveTime!.Value })
			.ToDictionaryAsync(x => x.GoalId, x => x.TimerId);

			var activeTimersId = activeGoals.Values.ToHashSet();
			var activeGoalsId = activeGoals.Keys.ToHashSet();

			await dbContext.ActiveGoalsTime.Where(x => activeTimersId.Contains(x.Id)).ExecuteUpdateAsync(setters => setters.SetProperty(x => x.End, data.BuildDate));
			await dbContext.ActiveGoals.Where(x => activeGoalsId.Contains(x.Id)).ExecuteUpdateAsync(setters => setters.SetProperty(x => x.CurrentlyActiveTime, x => null));

			await transaction.CommitAsync();
		}

		var goals = await dbContext.ActiveGoals.AsNoTracking()
			.Include(x => x.User).Where(x => x.User != null && x.User.Id == data.UserId)
			.Include(x => x.Contractor)
			.Include(x => x.Tasks.Where(x => x.Done))
			.Include(x => x.GoalTime)
			.ToListAsync();

		Dictionary<int, Contractor> attachedContractors = [];

		foreach (var goal in goals) {
			Contractor? attachedContractor = null;
			if (goal.Contractor is { Id: { } contractorId }) {
				if (!attachedContractors.TryGetValue(contractorId, out attachedContractor)) {
					attachedContractor = await dbContext.Contractors.FindAsync(contractorId);
					if (attachedContractor is not null) {
						attachedContractors.TryAdd(contractorId, attachedContractor);
					}
				}
			}

			JobContext context = new() {
				Contractor = attachedContractor,
				Comment = goal.Comment,
				Name = goal.Name
			};

			dbContext.Jobs.Update(context);

			foreach (var time in goal.GoalTime) {
				JobTime jobTime = new() {
					User = user,
					Context = context,
					Start = time.Start,
					End = time.End
				};

				dbContext.JobsTime.Update(jobTime);
			}
		}

		await dbContext.SaveChangesAsync();

		await using (var transaction = await dbContext.Database.BeginTransactionAsync()) {
			await dbContext.ActiveGoals
				.Include(x => x.User)
				.Include(x => x.GoalTime)
				.Where(x => x.User != null && x.User.Id == data.UserId)
				.ExecuteDeleteAsync();

			await dbContext.ToDos
				.Include(x => x.User).Where(x => x.User != null && x.User.Id == data.UserId)
				.Include(x => x.Goal)
				.Include(x => x.ActiveGoal)
				.Where(x => x.Done && x.Goal != null && x.ActiveGoal != null)
				.ExecuteDeleteAsync();

			await transaction.CommitAsync();
		}

		return true;
	}
}
