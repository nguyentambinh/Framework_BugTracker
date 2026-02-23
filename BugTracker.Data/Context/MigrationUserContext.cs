using BugTracker.Core.Interfaces;

namespace BugTracker.Infrastructure.Context
{
    public class MigrationUserContext : IUserContext
    {
        public int? UserId => null;
        public string UserName => "system:migration";

        public void SignIn(int userId, string userName)
        {
            // intentionally empty
        }

        public void SignOut()
        {
            // intentionally empty
        }
    }
}