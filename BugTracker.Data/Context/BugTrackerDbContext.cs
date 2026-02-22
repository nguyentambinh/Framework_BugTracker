using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using BugTracker.Core.Entities;
using BugTracker.Data.Mappings;
using BugTracker.Core.Base;
using BugTracker.Common.Helpers;
using Newtonsoft.Json;
using BugTracker.Core.Interfaces;
namespace BugTracker.Data.Context
{
    public class BugTrackerDbContext : DbContext
    {
        private readonly IUserContext _userContext;

        public BugTrackerDbContext(IUserContext userContext)
            : base("BugTrackerConnection")
        {
            _userContext = userContext;
        }

        public DbSet<Bug> Bugs { get; set; }
        public DbSet<BugGroup> BugGroups { get; set; }
        public DbSet<ApplicationUser> Users { get; set; }
        public DbSet<AuditLog> AuditLogs { get; set; }
        public DbSet<Permission> Permissions { get; set; }

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
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<RolePermission>()
                .HasKey(rp => new { rp.RoleId, rp.PermissionId });

            modelBuilder.Entity<RolePermission>()
                .HasRequired(rp => rp.Role)
                .WithMany(r => r.RolePermissions)
                .HasForeignKey(rp => rp.RoleId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<RolePermission>()
                .HasRequired(rp => rp.Permission)
                .WithMany(p => p.RolePermissions)
                .HasForeignKey(rp => rp.PermissionId)
                .WillCascadeOnDelete(false);
        }
        public override int SaveChanges()
        {
            AddAuditLogs();
            return base.SaveChanges();
        }
        private void AddAuditLogs()
        {
            var entries = ChangeTracker.Entries()
                .Where(e =>
                    e.Entity is IAuditable &&
                    (e.State == EntityState.Added ||
                     e.State == EntityState.Modified ||
                     e.State == EntityState.Deleted));

            foreach (var entry in entries)
            {
                AuditLogs.Add(new AuditLog
                {
                    EntityName = entry.Entity.GetType().Name,
                    EntityId = entry.CurrentValues["Id"] != null
                                ? (int)entry.CurrentValues["Id"]
                                : 0,
                    Action = entry.State.ToString(),
                    Changes = BuildChanges(entry),
                    CreatedAt = DateTime.UtcNow,
                    UserId = CurrentUserContext.UserId,
                    UserName = CurrentUserContext.UserName,
                    IpAddress = CurrentUserContext.Ip
                });
            }
        }
        private string BuildChanges(DbEntityEntry entry)
        {
            var changes = new Dictionary<string, object>();

            foreach (var propName in entry.OriginalValues.PropertyNames)
            {
                var originalValue = entry.OriginalValues[propName];
                var currentValue = entry.CurrentValues[propName];

                if (!Equals(originalValue, currentValue))
                {
                    changes[propName] = new
                    {
                        Old = originalValue,
                        New = currentValue
                    };
                }
            }

            return JsonConvert.SerializeObject(changes);
        }

    }
}