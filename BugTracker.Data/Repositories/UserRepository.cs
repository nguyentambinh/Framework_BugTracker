using System.Linq;
using BugTracker.Core.Entities;
using BugTracker.Core.Interfaces;
using BugTracker.Data.Context;

namespace BugTracker.Data.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly BugTrackerDbContext _context;

        public UserRepository(BugTrackerDbContext context)
        {
            _context = context;
        }

        public ApplicationUser GetByUsername(string username)
        {
            return _context.Users
                           .FirstOrDefault(x => x.UserName == username);
        }

        public ApplicationUser GetById(int id)
        {
            return _context.Users.Find(id);
        }

        public void Add(ApplicationUser user)
        {
            _context.Users.Add(user);
        }

        public void Save()
        {
            _context.SaveChanges();
        }
    }
}