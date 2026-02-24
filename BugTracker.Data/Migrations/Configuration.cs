namespace BugTracker.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using BugTracker.Common.Helpers;
    using BugTracker.Core.Entities;
    using BugTracker.Core.Enums;
    using BugTracker.Data.Context;

    internal sealed class Configuration : DbMigrationsConfiguration<BugTrackerDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(BugTrackerDbContext context)
        {
            var now = DateTime.Now;

            var permissions = new[]
            {
                new Permission { Code = PermissionCode.Bug_Open, Description = "Quyền mở Bug" },
                new Permission { Code = PermissionCode.Bug_InProgress, Description = "Quyền xử lý Bug" },
                new Permission { Code = PermissionCode.Bug_Resolved, Description = "Quyền giải quyết Bug" },
                new Permission { Code = PermissionCode.Bug_Closed, Description = "Quyền đóng Bug" }
            };

            foreach (var p in permissions)
            {
                context.Permissions.AddOrUpdate(x => x.Code, p);
            }
            context.SaveChanges();

            var adminRole = context.Roles.FirstOrDefault(r => r.Name == "Admin") ?? context.Roles.Add(new Role { Name = "Admin" });
            var devRole = context.Roles.FirstOrDefault(r => r.Name == "Developer") ?? context.Roles.Add(new Role { Name = "Developer" });
            var testerRole = context.Roles.FirstOrDefault(r => r.Name == "Tester") ?? context.Roles.Add(new Role { Name = "Tester" });
            context.SaveChanges();

            void AddRolePermission(Role role, PermissionCode code, bool open, bool progress, bool resolved, bool closed)
            {
                var permission = context.Permissions.First(p => p.Code == code);
                context.RolePermissions.AddOrUpdate(
                    rp => new { rp.RoleId, rp.PermissionId },
                    new RolePermission
                    {
                        RoleId = role.Id,
                        PermissionId = permission.Id,
                        CanOpen = open,
                        CanInProgress = progress,
                        CanFixed = resolved,
                        CanClosed = closed
                    }
                );
            }

            foreach (PermissionCode pc in Enum.GetValues(typeof(PermissionCode)))
            {
                AddRolePermission(adminRole, pc, true, true, true, true);
            }
            AddRolePermission(devRole, PermissionCode.Bug_InProgress, false, true, false, false);
            context.SaveChanges();

            ApplicationUser CreateUser(string username, string roleName)
            {
                var user = context.Users.FirstOrDefault(u => u.UserName == username);
                if (user != null) return user;

                var role = context.Roles.First(r => r.Name == roleName);
                user = new ApplicationUser
                {
                    UserName = username,
                    DisplayName = username.ToUpper(),
                    PasswordHash = PasswordHasher.Hash("123456"),
                    RoleId = role.Id,
                    IsActive = true,
                    CreatedAt = now 
                };
                context.Users.Add(user);
                context.SaveChanges();
                return user;
            }

            var adminUser = CreateUser("admin", "Admin");
            var devUser = CreateUser("dev", "Developer");
            var testerUser = CreateUser("tester", "Tester");

            // 5. SEED BUG GROUPS
            context.BugGroups.AddOrUpdate(
                g => g.Name,
                new BugGroup { Name = "Backend" },
                new BugGroup { Name = "Frontend" }
            );
            context.SaveChanges();


            var backGroup = context.BugGroups.First(g => g.Name == "Backend");

            context.Bugs.AddOrUpdate(
                b => b.Title,
                new Bug
                {
                    Title = "Lỗi Database Seed",
                    Description = "Fix lỗi datetime out of range",
                    Status = BugStatus.Open,
                    Priority = BugPriority.High,
                    BugGroupId = backGroup.Id,
                    CreatedByUserId = adminUser.Id,
                    AssignedToUserId = devUser.Id,
                    CreatedDate = now, 
                }
            );

            context.SaveChanges();
        }
    }
}