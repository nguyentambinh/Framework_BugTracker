using System.Data.Entity.Infrastructure;
using BugTracker.Core.Interfaces;
using BugTracker.Data.Context;

public class BugTrackerDbContextFactory
    : IDbContextFactory<BugTrackerDbContext>
{
    public BugTrackerDbContext Create()
    {
        var fakeUserContext = new MigrationUserContext();

        return new BugTrackerDbContext(fakeUserContext);
    }
}