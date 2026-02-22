using System.Data.Entity.ModelConfiguration;
using BugTracker.Core.Entities;

namespace BugTracker.Data.Mappings
{
    public class RolePermissionMap : EntityTypeConfiguration<RolePermission>
    {
        public RolePermissionMap()
        {
            HasKey(x => new { x.RoleId, x.PermissionId });

            HasRequired(x => x.Role)
                .WithMany(r => r.RolePermissions)
                .HasForeignKey(x => x.RoleId)
                .WillCascadeOnDelete(false);

            HasRequired(x => x.Permission)
                .WithMany(p => p.RolePermissions)
                .HasForeignKey(x => x.PermissionId)
                .WillCascadeOnDelete(false);
        }
    }
}