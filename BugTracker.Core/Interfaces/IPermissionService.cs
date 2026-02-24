using BugTracker.Core.Entities;
using BugTracker.Core.Enums;

public interface IPermissionService
{
    PermissionResult CanChangeStatus(Bug bug, BugStatus newStatus);
    bool CanChangeStatusFlag(BugStatus status);
}