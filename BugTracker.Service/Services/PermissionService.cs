using BugTracker.Core.Interfaces;
using BugTracker.Data.Context;
using System.Linq;
public class PermissionService : IPermissionService
{
    private readonly BugTrackerDbContext _db;
    private readonly IUserContext _userContext;

    public PermissionService(
        BugTrackerDbContext db,
        IUserContext userContext)
    {
        _db = db;
        _userContext = userContext;
    }

    public bool HasPermission(string permissionCode)
    {
        var userId = _userContext.UserId;
        if (!userId.HasValue) return false;

        return _db.Users
            .Where(u => u.Id == userId.Value)
            .SelectMany(u => u.Role.RolePermissions)
            .Any(rp => rp.Permission.Code == permissionCode);
    }
}