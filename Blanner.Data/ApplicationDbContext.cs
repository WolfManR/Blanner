using Blanner.Data.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Blanner.Data {
	public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext<User>(options)
	{
	    public virtual DbSet<Goal> Goals { get; set; } = null!;
	    public virtual DbSet<ActiveGoal> ActiveGoals { get; set; } = null!;
	    public virtual DbSet<ActiveGoalTime> ActiveGoalsTime { get; set; } = null!;
	    public virtual DbSet<ToDo> ToDos { get; set; } = null!;
	    public virtual DbSet<Contractor> Contractors { get; set; } = null!;
	    public virtual DbSet<JobContext> Jobs { get; set; } = null!;
	    public virtual DbSet<JobTime> JobsTime { get; set; } = null!;

		protected override void OnModelCreating(ModelBuilder builder) {
			base.OnModelCreating(builder);

	        builder.Entity<Goal>(e => {
	            e.HasOne(x => x.Contractor).WithMany().OnDelete(DeleteBehavior.NoAction);
	            e.HasMany(x => x.Tasks).WithOne(x => x.Goal).OnDelete(DeleteBehavior.SetNull);
	            e.HasOne(x => x.ActiveGoal).WithOne(x => x.Goal).OnDelete(DeleteBehavior.SetNull);
	            e.HasOne(x => x.User).WithMany().OnDelete(DeleteBehavior.NoAction);
			});

	        builder.Entity<ActiveGoal>(e => {
	            e.HasOne(x => x.Contractor).WithMany().OnDelete(DeleteBehavior.NoAction);
	            e.HasMany(x => x.Tasks).WithOne(x => x.ActiveGoal).OnDelete(DeleteBehavior.SetNull);
	            e.HasOne(x => x.Goal).WithOne(x => x.ActiveGoal).HasForeignKey<ActiveGoal>().OnDelete(DeleteBehavior.Cascade);
	            e.HasMany(x => x.GoalTime).WithOne(x => x.ActiveGoal).OnDelete(DeleteBehavior.SetNull);
				e.HasOne(x => x.User).WithMany().OnDelete(DeleteBehavior.NoAction);
			});

	        builder.Entity<ToDo>(e => {
	            e.HasOne(x => x.ActiveGoal).WithMany(x => x.Tasks).OnDelete(DeleteBehavior.NoAction);
	            e.HasOne(x => x.Goal).WithMany(x => x.Tasks).OnDelete(DeleteBehavior.NoAction);
                e.HasOne(x => x.User).WithMany().OnDelete(DeleteBehavior.NoAction);
            });

	        builder.Entity<ActiveGoalTime>(e => {
	            e.HasOne(x => x.ActiveGoal).WithMany(x => x.GoalTime).OnDelete(DeleteBehavior.Cascade);
	        });

	        builder.Entity<Contractor>(e => {
	            e.HasMany<Goal>().WithOne(x => x.Contractor).OnDelete(DeleteBehavior.SetNull);
	            e.HasMany<ActiveGoal>().WithOne(x => x.Contractor).OnDelete(DeleteBehavior.SetNull);
	            e.HasMany<JobContext>().WithOne(x => x.Contractor).OnDelete(DeleteBehavior.SetNull);
	        });

	        builder.Entity<JobContext>(e => {
	            e.HasOne(x => x.Contractor).WithMany().OnDelete(DeleteBehavior.NoAction);
	            e.HasMany<JobTime>().WithOne(x => x.Context).OnDelete(DeleteBehavior.Cascade);
            });

	        builder.Entity<JobTime>(e => {
	            e.HasOne(x => x.Context).WithMany().OnDelete(DeleteBehavior.Cascade);
                e.HasOne(x => x.User).WithMany().OnDelete(DeleteBehavior.NoAction);
            });
		}
	}
}
