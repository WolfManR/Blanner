using Blanner.Data.Models;
using Blanner.Data.Models.WorksManagement;
using Microsoft.EntityFrameworkCore;

namespace Blanner.Data;
public class GoalsTemplatesRepository(ApplicationDbContext dbContext) {
	private readonly ApplicationDbContext _dbContext = dbContext;

	public IQueryable<GoalTemplate> Templates(string userId) {
		var query = _dbContext.GoalsTemplates.AsNoTracking().Include(x=>x.Contractor).Include(x => x.Contractors).AsQueryable();
		query = query.Include(x => x.User).Where(x => x.User != null && x.User.Id == userId);
		return query;
	}

	public Task<GoalTemplate> Template(int templateId) {
		return _dbContext.GoalsTemplates.AsNoTracking().Include(x => x.User).Include(x => x.Contractors).FirstAsync(x => x.Id == templateId);
	}

	public async ValueTask<GoalTemplate?> Create(string userId, string text) {
		User? user = await _dbContext.Users.FindAsync(userId);
		if (user is null) return null;

		GoalTemplate template = new(text.NullOrEmpty() ? "Noname" : text.Cut(100), user) {
			Comment = text
		};

		_dbContext.GoalsTemplates.Add(template);
		await _dbContext.SaveChangesAsync();

		return template;
	}

	public async ValueTask<GoalTemplate?> AssignContractors(long templateId, int[] contractorsId) {
		GoalTemplate? template = await _dbContext.GoalsTemplates.Include(x => x.User).Include(x => x.Contractors).FirstOrDefaultAsync(x => x.Id == templateId);
		if (template is null) return null;

		var contractorsToDelete = (from existed in template.Contractors
								   join id in contractorsId on existed.Id equals id into temp
								   from id in temp.DefaultIfEmpty()
								   where id == default
								   select existed).ToHashSet();

		template.Contractors.RemoveAll(contractorsToDelete.Contains);

		var contractorsToAdd = from id in contractorsId
							   join existed in template.Contractors.Select(x => x.Id) on id equals existed into temp
							   from existed in temp.DefaultIfEmpty()
							   where existed == default
							   select id;

		foreach (var toAdd in contractorsToAdd) {
			Contractor? contractor = await _dbContext.Contractors.FindAsync(toAdd);
			if (contractor is not null)
				template.Contractors.Add(contractor);
		}

		await _dbContext.SaveChangesAsync();

		return template;
	}

	public async ValueTask<GoalTemplate?> Save(string userId, string name, string comment, int[] contractorId) {
		User? user = await _dbContext.Users.FindAsync(userId);
		if (user is null) return null;

		GoalTemplate template = new(name, user)
		{
			Comment = comment
		};

		//if(contractorId > 0 && await _dbContext.Contractors.FindAsync(contractorId) is { } contractor) {
		//	template.Contractors.Add(contractor);
		//}

		_dbContext.GoalsTemplates.Add(template);
		await _dbContext.SaveChangesAsync();
	
		return template;
	}

	public async Task<GoalTemplate> Update(int templateId, string name, string Comment, int[] contractorsId) {
		GoalTemplate template = await _dbContext.GoalsTemplates
			.Include(x => x.Contractors)
			.FirstAsync(x => x.Id == templateId);

		template.Name = name;
		template.Comment = Comment;

		var contractorsToDelete = (from existed in template.Contractors
								   join id in contractorsId on existed.Id equals id into temp
								   from id in temp.DefaultIfEmpty()
								   where id == default
								   select existed).ToHashSet();

		template.Contractors.RemoveAll(contractorsToDelete.Contains);

		var contractorsToAdd = from id in contractorsId
							   join existed in template.Contractors.Select(x=>x.Id) on id equals existed into temp
							   from existed in temp.DefaultIfEmpty()
							   where existed == default
							   select id;

        foreach (var toAdd in contractorsToAdd)
        {
			Contractor? contractor = await _dbContext.Contractors.FindAsync(toAdd);
			if (contractor is not null)
				template.Contractors.Add(contractor);
		}

		await _dbContext.SaveChangesAsync();

		return template;
	}

	public async Task<GoalTemplate?> Delete(int templateId) {
        GoalTemplate? template = await _dbContext.GoalsTemplates.FirstOrDefaultAsync(x => x.Id == templateId);
		if(template is null) return null;

		_dbContext.Remove(template);

		await _dbContext.SaveChangesAsync();

		return template;
	}

	public async Task<Goal> Activate(int templateId, DateTimeOffset activateDate) {
		GoalTemplate? template = await _dbContext.GoalsTemplates
			.Include(x => x.Contractors)
			.Include(x => x.User)
			.FirstOrDefaultAsync(x => x.Id == templateId) ?? throw new InvalidOperationException("Template not exist");
        Goal goal = new(template, activateDate);
		_dbContext.Add(goal);

		await _dbContext.SaveChangesAsync();

		return goal;
	}
}
