using BugTracker.Core.Enums;
public interface IAuthorizationService
{
    bool CanChangeStatus(int userId, PermissionCode permissionCode, BugStatus status);
}