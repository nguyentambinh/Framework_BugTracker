using BugTracker.Data.Context;
using BugTracker.Core.Interfaces;
using BugTracker.Core.Entities;
using BugTracker.Core.Enums;
using System.Linq;
public class AuthorizationService : IAuthorizationService
{
    private readonly BugTrackerDbContext _context;

    public AuthorizationService(BugTrackerDbContext context)
    {
        _context = context;
    }

    public bool CanChangeStatus(int userId, PermissionCode permissionCode, BugStatus status)
    {
        var user = _context.Users.Find(userId);
        if (user == null) return false;

        var rp = _context.RolePermissions
            .FirstOrDefault(x =>
                x.RoleId == user.RoleId &&
                x.Permission.Code == permissionCode);

        if (rp == null) return false;

        switch (status)
        {
            case BugStatus.Open:
                return rp.CanOpen;

            case BugStatus.InProgress:
                return rp.CanInProgress;

            case BugStatus.Fixed:
                return rp.CanFixed;

            case BugStatus.Closed:
                return rp.CanClosed;

            default:
                return false;
        }
    }
}