using BugTracker.Core.Entities;
using BugTracker.Core.Interfaces;

namespace BugTracker.Infrastructure.Context
{
    public class MigrationUserContext : IUserContext
    {
        public int? UserId => null;

        public string UserName => "system:migration";

        public string IP => "SYSTEM";

        public void SignIn(int userId, string userName)
        {
            // intentionally empty
        }

        public void SignOut()
        {
            // intentionally empty
        }
        public ApplicationUser GetCurrentUser()
        {
            return null;
        }
    }
}