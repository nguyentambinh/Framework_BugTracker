using System.Data.Entity;
using BugTracker.Core.Entities;
using BugTracker.Data.Mappings;

namespace BugTracker.Data.Context
{
    public class BugTrackerDbContext : DbContext
    {
        public BugTrackerDbContext()
            : base("BugTrackerConnection")
        {
        }

        public DbSet<Bug> Bugs { get; set; }
        public DbSet<BugGroup> BugGroups { get; set; }
        public DbSet<ApplicationUser> Users { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Bug>()
                .HasRequired(b => b.CreatedByUser)
                .WithMany(u => u.CreatedBugs)
                .HasForeignKey(b => b.CreatedByUserId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Bug>()
                .HasOptional(b => b.AssignedToUser)
                .WithMany(u => u.AssignedBugs)
                .HasForeignKey(b => b.AssignedToUserId)
                .WillCascadeOnDelete(false);
        }
    }
}