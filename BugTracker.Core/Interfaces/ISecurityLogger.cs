namespace BugTracker.Core.Interfaces
{
    public interface ISecurityLogger
    {
        void LogLoginSuccess(string username);
        void LogLoginFail(string username, string reason);
    }
}