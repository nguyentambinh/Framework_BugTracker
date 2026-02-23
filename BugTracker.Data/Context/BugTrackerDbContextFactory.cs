using System.Data.Entity.Infrastructure;
using BugTracker.Core.Interfaces;
using BugTracker.Data.Context;
using BugTracker.Infrastructure.Context; 

namespace BugTracker.Data.Context
{
    public class BugTrackerDbContextFactory
        : IDbContextFactory<BugTrackerDbContext>
    {
        public BugTrackerDbContext Create()
        {
            IUserContext userContext = new MigrationUserContext();

            return new BugTrackerDbContext(userContext);
        }
    }
}