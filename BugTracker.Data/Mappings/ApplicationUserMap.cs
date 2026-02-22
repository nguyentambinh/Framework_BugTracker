using System.Data.Entity.ModelConfiguration;
using BugTracker.Core.Entities;

namespace BugTracker.Data.Mappings
{
    public class ApplicationUserMap : EntityTypeConfiguration<ApplicationUser>
    {
        public ApplicationUserMap()
        {
            ToTable("Users");

            HasKey(x => x.Id);

            Property(x => x.UserName)
                .IsRequired()
                .HasMaxLength(50);
        }
    }
}