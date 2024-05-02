using Blanner.Data.Models;

using Microsoft.EntityFrameworkCore;

namespace Blanner.Data;
public class UsersRepository(ApplicationDbContext dbContext) {
	private readonly ApplicationDbContext _dbContext = dbContext;

	public IQueryable<User> Users() {
		var query = _dbContext.Users.AsNoTracking();
		return query;
	}

	public Task<User?> User(string userId) {
		return _dbContext.Users.AsNoTracking().FirstOrDefaultAsync(x => x.Id == userId);
	}

	public Task<List<ActiveUserWorkData>> WorkingUsers() {
		var query = _dbContext.ActiveGoals.AsNoTracking()
			.Include(x => x.User)
			.Where(x => x.User != null && x.CurrentlyActiveTime != null)
			.GroupBy(x => x.User)
			.Select(x => new ActiveUserWorkData {
				User = new UserInfoData{ Id = x.Key!.Id, Name = x.Key.UserName },
				WorkData = x.Select(d => new WorkData { Id = d.Id, Name = d.Name }).ToList()
			})
			.ToListAsync();
		return query;
	}
}
