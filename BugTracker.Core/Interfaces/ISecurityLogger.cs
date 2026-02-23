namespace BugTracker.Core.Interfaces
{
    public interface ISecurityLogger
    {
        void LogLoginSuccess(int userId);
        void LogLoginFail(string username);
    }
}