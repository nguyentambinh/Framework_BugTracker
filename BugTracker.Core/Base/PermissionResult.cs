public class PermissionResult
{
    public bool Success { get; set; }
    public string Message { get; set; }

    public static PermissionResult Allow()
        => new PermissionResult { Success = true };

    public static PermissionResult Deny(string message)
        => new PermissionResult { Success = false, Message = message };
}