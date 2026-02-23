using BugTracker.Core.Entities;
namespace BugTracker.Core.Interfaces
{
    public interface IUserRepository
    {
        ApplicationUser GetByUsername(string username);
    }
}