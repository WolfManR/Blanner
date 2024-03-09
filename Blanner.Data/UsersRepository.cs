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
}
