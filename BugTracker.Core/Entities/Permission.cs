using System.Collections.Generic;

public class Permission
{
    public int Id { get; set; }
    public string Code { get; set; }
    public string Description { get; set; }

    public virtual ICollection<RolePermission> RolePermissions { get; set; }
}