using System.Data.Entity.ModelConfiguration;
using BugTracker.Core.Entities;

namespace BugTracker.Data.Mappings
{
    public class BugMap : EntityTypeConfiguration<Bug>
    {
        public BugMap()
        {
            ToTable("Bugs");

            HasKey(x => x.Id);

            Property(x => x.Title)
                .IsRequired()
                .HasMaxLength(200);

            HasOptional(x => x.BugGroup)
                .WithMany(g => g.Bugs)
                .HasForeignKey(x => x.BugGroupId);

            HasRequired(x => x.CreatedByUser)
                .WithMany(u => u.CreatedBugs)
                .HasForeignKey(x => x.CreatedByUserId)
                .WillCascadeOnDelete(false);

            HasOptional(x => x.AssignedToUser)
                .WithMany(u => u.AssignedBugs)
                .HasForeignKey(x => x.AssignedToUserId)
                .WillCascadeOnDelete(false);
        }
    }
}