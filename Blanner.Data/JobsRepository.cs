using Blanner.Data.Models;

using Microsoft.EntityFrameworkCore;

namespace Blanner.Data;

public class JobsRepository(ApplicationDbContext dbContext) {
    private readonly ApplicationDbContext _dbContext = dbContext;

    public async Task<List<JobHeaderData>> List(string userId, DateTimeOffset start, DateTimeOffset end) {
        var providerIsSQLite = _dbContext.Database.ProviderName == "Microsoft.EntityFrameworkCore.Sqlite";
        List<JobHeaderData> result;
        if (providerIsSQLite) {
            var predata = await _dbContext.JobsTime.AsNoTracking()
                .Include(x => x.User)
                .Where(x => x.User != null && x.User.Id == userId)
                .Include(x => x.Context).ThenInclude(x => x.Contractor)
                .ToListAsync();

            var data = predata.Where(x => x.End > start && x.Start < end)
                .Select(x => x.Context).DistinctBy(x=>x.Id)
                .Select(x => new {
                    Context = x,
                    Start = _dbContext.JobsTime.Include(x => x.Context).Where(t => t.Context == x).Select(t => t.Start).ToList().Min(),
                    End = _dbContext.JobsTime.Include(x => x.Context).Where(t => t.Context == x).Select(t => t.End).ToList().Max(),
                    TotalTime = _dbContext.JobsTime.Include(x => x.Context).Where(t => t.Context == x).Select(t => t.End - t.Start).ToList()
                }).ToList();

            result = data
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
        }
        else {
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

            result = data
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
        }
       
        return result;
    }

    public async Task<JobDetailsData?> Details(int id) {
		var providerIsSQLite = _dbContext.Database.ProviderName == "Microsoft.EntityFrameworkCore.Sqlite";
		JobDetailsData? result;
		if (providerIsSQLite) {
            var predata = await _dbContext.JobsTime
            .AsNoTracking()
            .Include(x => x.Context).ThenInclude(x => x.Contractor)
            .Where(x => x.Context.Id == id)
            .Select(x => x.Context).Distinct()
            .ToListAsync();

            var data = predata.Select(x => new {
                Context = x,
                User = _dbContext.JobsTime.Include(x => x.Context).Include(x => x.User).Where(t => t.Context == x).Select(x => x.User).First(),

				Start = _dbContext.JobsTime.Include(x => x.Context).Where(t => t.Context == x).Select(t => t.Start).ToList().Min(),
				End = _dbContext.JobsTime.Include(x => x.Context).Where(t => t.Context == x).Select(t => t.End).ToList().Max(),
				Time = _dbContext.JobsTime.Include(x => x.Context).Where(t => t.Context == x).Select(t => new JobDetailsTimeData {
                    Id = t.Id,
                    Start = t.Start,
                    End = t.End
                })
            }).ToList();

            result = data.Select(x => new JobDetailsData {
				Id = x.Context.Id,
				Start = x.Start,
				End = x.End,
				Contractor = x.Context.Contractor,
				User = x.User,
				Name = x.Context.Name,
				Comment = x.Context.Comment,
				Marked = x.Context.Marked,
				MarkComment = x.Context.MarkComment,
				Time = x.Time.ToList()
			})
			.FirstOrDefault();
		}
		else {
			result = await _dbContext.JobsTime
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
                })
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
				Time = x.Time.ToList()
			})
            .FirstOrDefaultAsync();
		}

		return result;
    }
}
