public class RolePermission
{
    public int RoleId { get; set; }
    public int PermissionId { get; set; }

    public bool CanOpen { get; set; }
    public bool CanInProgress { get; set; }
    public bool CanFixed { get; set; }
    public bool CanClosed { get; set; }

    public virtual Role Role { get; set; }
    public virtual Permission Permission { get; set; }
}
