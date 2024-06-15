using Blanner.Data.Models;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Blanner.Data; 
public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext<User>(options)
{
    public virtual DbSet<GoalTemplate> GoalsTemplates { get; set; } = null!;
    public virtual DbSet<Goal> Goals { get; set; } = null!;
    public virtual DbSet<ActiveGoalTime> ActiveGoalsTime { get; set; } = null!;
    public virtual DbSet<ToDo> ToDos { get; set; } = null!;
    public virtual DbSet<Contractor> Contractors { get; set; } = null!;
    public virtual DbSet<JobContext> Jobs { get; set; } = null!;
    public virtual DbSet<JobChanges> JobChanges { get; set; } = null!;
    public virtual DbSet<JobTime> JobsTime { get; set; } = null!;

	protected override void OnModelCreating(ModelBuilder builder) {
		base.OnModelCreating(builder);

        builder.Entity<GoalTemplate>(e => {
            e.HasOne(x => x.User).WithMany().HasForeignKey(x => x.UserId).OnDelete(DeleteBehavior.Cascade);
            e.HasOne(x => x.Contractor).WithMany().HasForeignKey(x => x.ContractorId).OnDelete(DeleteBehavior.SetNull);
		});

        builder.Entity<Goal>(e => {
			e.HasOne(x => x.User).WithMany().HasForeignKey(x => x.UserId).OnDelete(DeleteBehavior.NoAction);
            e.HasOne(x => x.Contractor).WithMany().HasForeignKey(x => x.ContractorId).OnDelete(DeleteBehavior.SetNull);
            e.HasMany(x => x.Tasks).WithOne(x => x.Goal).OnDelete(DeleteBehavior.SetNull);
            e.HasMany(x => x.GoalTime).WithOne(x => x.ActiveGoal).OnDelete(DeleteBehavior.SetNull);
		});

        builder.Entity<ToDo>(e => {
            e.HasOne(x => x.Goal).WithMany(x => x.Tasks).OnDelete(DeleteBehavior.SetNull);
            e.HasOne(x => x.User).WithMany().OnDelete(DeleteBehavior.SetNull);
        });

        builder.Entity<ActiveGoalTime>(e => {
            e.HasOne(x => x.ActiveGoal).WithMany(x => x.GoalTime).OnDelete(DeleteBehavior.Cascade);
        });

        builder.Entity<Contractor>(e => {
            e.HasMany<GoalTemplate>().WithOne(x => x.Contractor).OnDelete(DeleteBehavior.SetNull);
            e.HasMany<Goal>().WithOne(x => x.Contractor).OnDelete(DeleteBehavior.SetNull);
            e.HasMany<JobContext>().WithOne(x => x.Contractor).OnDelete(DeleteBehavior.SetNull);
        });

        builder.Entity<JobContext>(e => {
            e.HasOne(x => x.User).WithMany().OnDelete(DeleteBehavior.SetNull);
            e.HasOne(x => x.Contractor).WithMany().OnDelete(DeleteBehavior.NoAction);
            e.HasMany(x => x.Time).WithOne(x => x.Context).OnDelete(DeleteBehavior.SetNull);
            e.HasMany(x => x.Changes).WithOne(x => x.Context).OnDelete(DeleteBehavior.SetNull);
            e.Property(x => x.Date).HasDefaultValue(DateOnly.FromDateTime(new DateTime(2024, 04, 07)));
        });

        builder.Entity<JobTime>(e => {
            e.HasOne(x => x.Context).WithMany(x => x.Time).OnDelete(DeleteBehavior.Cascade);
            e.HasOne(x => x.JobChange).WithMany(x => x.Time).OnDelete(DeleteBehavior.NoAction);
            e.HasOne(x => x.User).WithMany().OnDelete(DeleteBehavior.NoAction);
        });

        builder.Entity<JobChanges>(e => {
            e.HasOne(x => x.Context).WithMany(x => x.Changes).OnDelete(DeleteBehavior.Cascade);
			e.HasMany(x => x.Time).WithOne(x => x.JobChange).OnDelete(DeleteBehavior.NoAction);
        });
	}
}
