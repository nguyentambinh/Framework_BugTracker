using BugTracker.Core.Interfaces;

public class MigrationUserContext : IUserContext
{
    public int? UserId => null;
    public string UserName => "migration";
    public string IpAddress => "127.0.0.1";
}