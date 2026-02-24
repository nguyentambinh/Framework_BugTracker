using BugTracker.Core.Entities;
using BugTracker.Core.Interfaces;
using System.Linq;
using System.Data.Entity;
using BugTracker.Data.Context;

namespace BugTracker.Data.Services
{
    public class UserPermissionManager : IUserPermissionManager
    {
        private readonly BugTrackerDbContext _context;
        private ApplicationUser _user;

        public UserPermissionManager(BugTrackerDbContext context)
        {
            _context = context;
        }

        public IUserPermissionManager ForUser(int userId)
        {
            _user = _context.Users
                .Include(u => u.Role.RolePermissions)
                .FirstOrDefault(u => u.Id == userId);
            return this;
        }

        public IUserPermissionManager SetAdmin()
        {
            return SetRole(true, true, true, true);
        }

        public IUserPermissionManager SetDev()
        {
            return SetRole(true, true, true, false); 
        }
        public IUserPermissionManager SetTester()
        {
            return SetRole(true, false, false, false);
        }
        public IUserPermissionManager SetRole(bool canOpen, bool canInProgress, bool canFixed, bool canClosed)
        {
            if (_user?.Role == null) return this;

            var permission = _user.Role.RolePermissions.FirstOrDefault();
            if (permission == null)
            {
                permission = new RolePermission { RoleId = _user.RoleId ?? 0 };
                _context.RolePermissions.Add(permission);
            }

            permission.CanOpen = canOpen;
            permission.CanInProgress = canInProgress;
            permission.CanFixed = canFixed;
            permission.CanClosed = canClosed;

            return this;
        }

        public void Save()
        {
            _context.SaveChanges();
        }
    }
}