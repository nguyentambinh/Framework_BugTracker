using BugTracker.Core.Entities;

namespace BugTracker.Core.Interfaces
{
    public interface IUserRepository
    {
        ApplicationUser GetByUsername(string username);
        ApplicationUser GetById(int id);
        void Add(ApplicationUser user);
        void Save();
    }
}