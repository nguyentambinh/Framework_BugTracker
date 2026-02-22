namespace BugTracker.Data.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<BugTracker.Data.Context.BugTrackerDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(BugTracker.Data.Context.BugTrackerDbContext context)
        {
            context.Permissions.AddOrUpdate(
                p => p.Code,
                new Permission { Code = "BUG_CREATE", Description = "Create bug" },
                new Permission { Code = "BUG_EDIT", Description = "Edit bug" },
                new Permission { Code = "BUG_DELETE", Description = "Delete bug" }
            );
        }
    }
}
