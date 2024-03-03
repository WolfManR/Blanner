using Blanner.Data.Models;

using Microsoft.EntityFrameworkCore;

namespace Blanner.Data; 
public class ContractorsRepository(ApplicationDbContext dbContext) {
	private readonly ApplicationDbContext _dbContext = dbContext;

	public Task<List<Contractor>> List() {
		return _dbContext.Contractors.OrderBy(x => x.Name).AsNoTracking().ToListAsync();
    }

	public async Task<Contractor> Add(string name, DateTimeOffset createdAt) {
        var existed = await _dbContext.Contractors.FirstOrDefaultAsync(x => x.Name == name);
        if (existed is not null) throw new InvalidOperationException("Trying store existed contractor");

		Contractor contractor = new(name) { CreatedAt = createdAt };
		_dbContext.Contractors.Add(contractor);
		await _dbContext.SaveChangesAsync();

		return contractor;
    }

	public async Task<Contractor?> Save(int id, string name, DateTimeOffset updatedAt) {
		Contractor? contractor = await _dbContext.Contractors.FindAsync(id);
		if (contractor is null) return null;

		contractor.Name = name;
		contractor.UpdatedAt = updatedAt;
	
		await _dbContext.SaveChangesAsync();
		return contractor;
	}

	public async Task<Contractor?> Destroy(int id) {
        Contractor? contractor = await _dbContext.Contractors.FindAsync(id);
        if (contractor is null) return null;
        
		_dbContext.Remove(contractor);
		await _dbContext.SaveChangesAsync();

		return contractor;
	}
}
