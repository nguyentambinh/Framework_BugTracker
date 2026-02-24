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
        public DbSet<Role> Roles { get; set; }
        public DbSet<RolePermission> RolePermissions { get; set; }

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
            var now = DateTime.UtcNow;

            var entries = ChangeTracker.Entries()
                .Where(e =>
                    e.Entity is IAuditable &&
                    (e.State == EntityState.Added ||
                     e.State == EntityState.Modified ||
                     e.State == EntityState.Deleted));

            foreach (var entry in entries)
            {
                var entityName = entry.Entity.GetType().Name;

                var entityId = 0;
                if (entry.State != EntityState.Added)
                {
                    entityId = (int)entry.OriginalValues["Id"];
                }

                AuditLogs.Add(new AuditLog
                {
                    EntityName = entityName,
                    EntityId = entityId,
                    Action = entry.State.ToString(),
                    Changes = BuildChanges(entry),

                    UserId = _userContext.UserId,
                    UserName = _userContext.UserName,
                    IpAddress = _userContext.IP,

                    ChangedAt = now,
                    CreatedAt = now
                });
            }
        }
        private object ConvertEnumIfNeeded(DbEntityEntry entry, string propName, object value)
        {
            if (value == null)
                return null;

            var propInfo = entry.Entity.GetType().GetProperty(propName);
            if (propInfo == null)
                return value;

            if (propInfo.PropertyType.IsEnum)
                return Enum.GetName(propInfo.PropertyType, value);

            return value;
        }
        private string BuildChanges(DbEntityEntry entry)
        {
            var changes = new Dictionary<string, object>();

            // ADD
            if (entry.State == EntityState.Added)
            {
                foreach (var propName in entry.CurrentValues.PropertyNames)
                {
                    var newValue = entry.CurrentValues[propName];

                    changes[propName] = new
                    {
                        Old = (object)null,
                        New = ConvertEnumIfNeeded(entry, propName, newValue)
                    };
                }
            }
            // DELETE
            else if (entry.State == EntityState.Deleted)
            {
                foreach (var propName in entry.OriginalValues.PropertyNames)
                {
                    var oldValue = entry.OriginalValues[propName];

                    changes[propName] = new
                    {
                        Old = ConvertEnumIfNeeded(entry, propName, oldValue),
                        New = (object)null
                    };
                }
            }
            // MODIFY
            else if (entry.State == EntityState.Modified)
            {
                foreach (var propName in entry.OriginalValues.PropertyNames)
                {
                    var oldValue = entry.OriginalValues[propName];
                    var newValue = entry.CurrentValues[propName];

                    if (Equals(oldValue, newValue))
                        continue;

                    changes[propName] = new
                    {
                        Old = ConvertEnumIfNeeded(entry, propName, oldValue),
                        New = ConvertEnumIfNeeded(entry, propName, newValue)
                    };
                }
            }

            return changes.Any()
                ? JsonConvert.SerializeObject(changes)
                : null;
        }
    }
}