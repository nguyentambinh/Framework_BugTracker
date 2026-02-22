namespace BugTracker.Core.Interfaces
{
    public interface IUserContext
    {
        int? UserId { get; }
        string UserName { get; }
        string IpAddress { get; }
    }
}