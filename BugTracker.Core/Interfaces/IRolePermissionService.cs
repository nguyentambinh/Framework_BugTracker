using BugTracker.Core.Enums;
public interface IRolePermissionService
{
    void UpdatePermission(
        int roleId,
        int permissionId,
        BugStatus status,
        bool allowed
    );
}