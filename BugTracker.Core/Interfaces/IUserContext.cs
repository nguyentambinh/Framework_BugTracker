namespace BugTracker.Core.Interfaces
{
    public interface IUserContext
    {
        int? UserId { get; }
        string UserName { get; }

        void SignIn(int userId, string userName);
        void SignOut();
    }
}