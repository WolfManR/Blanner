using Blanner.Data.Models;
using Blanner.Data.Models.WorksManagement;
using Microsoft.EntityFrameworkCore;

namespace Blanner.Data;
public class GoalsTemplatesRepository(ApplicationDbContext dbContext) {
	private readonly ApplicationDbContext _dbContext = dbContext;

	public IQueryable<GoalTemplate> Templates(string userId) {
		var query = _dbContext.GoalsTemplates.AsNoTracking().Include(x => x.Contractor).AsQueryable();
		query = query.Include(x => x.User).Where(x => x.User != null && x.User.Id == userId);
		return query;
	}

	public Task<GoalTemplate> Template(int templateId) {
		return _dbContext.GoalsTemplates.AsNoTracking().Include(x => x.User).Include(x => x.Contractor).FirstAsync(x => x.Id == templateId);
	}

	public async ValueTask<GoalTemplate?> Save(string userId, string name, string comment, int? contractorId) {
		User? user = await _dbContext.Users.FindAsync(userId);
		if (user is null) return null;

		GoalTemplate template = new(name, user)
		{
			Comment = comment,
			ContractorId = contractorId
		};

		_dbContext.GoalsTemplates.Add(template);
		await _dbContext.SaveChangesAsync();
	
		return template;
	}

	public async Task<GoalTemplate> Update(int templateId, string name, string Comment, int? contractorId) {
		GoalTemplate template = await _dbContext.GoalsTemplates
			.Include(x => x.Contractor)
			.FirstAsync(x => x.Id == templateId);

		template.Name = name;
		template.Comment = Comment;
		
		switch (template.Contractor is null, contractorId is null) {
			case (false, true):
				template.Contractor = null;
				break;
			case (_, false) when (template.Contractor?.Id != contractorId):
				Contractor? contractor = await _dbContext.Contractors.FindAsync(contractorId);
				if (contractor is null) break;
				template.Contractor = contractor;
				break;
			default:
				break;
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
			.Include(x => x.Contractor)
			.Include(x => x.User)
			.FirstOrDefaultAsync(x => x.Id == templateId) ?? throw new InvalidOperationException("Template not exist");
        Goal goal = new(template, activateDate);
		_dbContext.Add(goal);

		await _dbContext.SaveChangesAsync();

		return goal;
	}
}
