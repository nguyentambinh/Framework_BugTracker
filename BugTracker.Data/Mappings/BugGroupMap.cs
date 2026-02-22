using System.Data.Entity.ModelConfiguration;
using BugTracker.Core.Entities;

namespace BugTracker.Data.Mappings
{
    public class BugGroupMap : EntityTypeConfiguration<BugGroup>
    {
        public BugGroupMap()
        {
            ToTable("BugGroups");
            HasKey(x => x.Id);

            Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(100);
        }
    }
}