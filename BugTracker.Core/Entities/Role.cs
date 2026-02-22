using System.Collections.Generic;

public class Role
{
    public int Id { get; set; }
    public string Name { get; set; }

    public virtual ICollection<RolePermission> RolePermissions { get; set; }
}