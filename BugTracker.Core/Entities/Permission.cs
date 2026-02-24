using System.Collections.Generic;
using BugTracker.Core.Enums;
public class Permission
{
    public int Id { get; set; }
    public PermissionCode Code { get; set; }    
    public string Description { get; set; }

    public virtual ICollection<RolePermission> RolePermissions { get; set; }
}