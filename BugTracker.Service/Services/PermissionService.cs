using BugTracker.Core.Enums;
using BugTracker.Core.Entities;
using System.Linq;
using System.Diagnostics;
using BugTracker.Core.Interfaces;
using BugTracker.Data.Context;
using System.Data.Entity;
using System;
using BugTracker.Core.Workflow;

public class PermissionService : IPermissionService
{
    private readonly IUserContext _userContext;

    public PermissionService(IUserContext userContext)
    {
        _userContext = userContext;
    }

    private ApplicationUser GetUserDirectly()
    {
        try
        {
            int currentUserId = _userContext.UserId ?? 0;
            Debug.WriteLine($"[DEBUG] PermissionService - UserId from Context: {currentUserId}");

            if (currentUserId == 0)
            {
                Debug.WriteLine("[DEBUG] PermissionService - UserId is 0 or Null");
                return null;
            }

            using (var db = new BugTrackerDbContext(_userContext))
            {
                db.Configuration.LazyLoadingEnabled = false;
                db.Configuration.ProxyCreationEnabled = false;

                var user = db.Users
                    .Include(u => u.Role)
                    .Include("Role.RolePermissions")
                    .Include("Role.RolePermissions.Permission")
                    .AsNoTracking()
                    .FirstOrDefault(u => u.Id == currentUserId);

                if (user == null)
                {
                    Debug.WriteLine($"[DEBUG] PermissionService - No user found in DB with ID: {currentUserId}");
                }
                else
                {
                    Debug.WriteLine($"[DEBUG] PermissionService - User Found: {user.DisplayName}, Role: {user.Role?.Name}");
                }

                return user;
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine("[DEBUG] PermissionService - CRITICAL ERROR: " + ex.Message);
            return null;
        }
    }

    public bool CanChangeStatusFlag(BugStatus status)
    {
        var user = GetUserDirectly();
        if (user == null) return false;

        if (user.Role != null && string.Equals(user.Role.Name, "Admin", StringComparison.OrdinalIgnoreCase))
        {
            Debug.WriteLine($"[DEBUG] PermissionService - Admin detected, granting permission for {status}");
            return true;
        }

        var permissionCode = MapStatusToPermission(status);
        var rolePermission = user.Role?.RolePermissions
            ?.FirstOrDefault(rp => rp.Permission != null && rp.Permission.Code == permissionCode);

        if (rolePermission == null)
        {
            Debug.WriteLine($"[DEBUG] PermissionService - No RolePermission record found for {permissionCode}");
            return false;
        }

        bool hasPermission = false;
        switch (status)
        {
            case BugStatus.Open: hasPermission = rolePermission.CanOpen; break;
            case BugStatus.InProgress: hasPermission = rolePermission.CanInProgress; break;
            case BugStatus.Fixed: hasPermission = rolePermission.CanFixed; break;
            case BugStatus.Closed: hasPermission = rolePermission.CanClosed; break;
        }

        Debug.WriteLine($"[DEBUG] PermissionService - Check {status}: {hasPermission}");
        return hasPermission;
    }

    public PermissionResult CanChangeStatus(Bug bug, BugStatus newStatus)
    {
        var user = GetUserDirectly();
        if (user == null) return PermissionResult.Deny("Bạn chưa đăng nhập!");

        var workflow = BugWorkflow.CanTransition(bug.Status, newStatus);
        if (!workflow.IsValid)
            return PermissionResult.Deny(workflow.Message);

        if (user.Role?.Name == "Admin") return PermissionResult.Allow();

        var p = user.Role?.RolePermissions?.FirstOrDefault();
        if (p == null) return PermissionResult.Deny("Tài khoản của bạn chưa được phân quyền!");

        bool hasPermission = false;
        switch (newStatus)
        {
            case BugStatus.Open: hasPermission = p.CanOpen; break;
            case BugStatus.InProgress: hasPermission = p.CanInProgress; break;
            case BugStatus.Fixed: hasPermission = p.CanFixed; break;
            case BugStatus.Closed: hasPermission = p.CanClosed; break;
        }

        if (!hasPermission)
            return PermissionResult.Deny($"Role {user.Role.Name} không có quyền chuyển sang trạng thái {newStatus}");

        return PermissionResult.Allow();
    }

    private PermissionCode MapStatusToPermission(BugStatus status)
    {
        switch (status)
        {
            case BugStatus.Open: return PermissionCode.Bug_Open;
            case BugStatus.InProgress: return PermissionCode.Bug_InProgress;
            case BugStatus.Fixed: return PermissionCode.Bug_Resolved;
            case BugStatus.Closed: return PermissionCode.Bug_Closed;
            default: throw new Exception("Status không hợp lệ");
        }
    }
}