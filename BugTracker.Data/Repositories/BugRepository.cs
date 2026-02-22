using BugTracker.Core.Entities;
using BugTracker.Data.Context;

namespace BugTracker.Data.Repositories
{
    public class BugRepository : EfRepository<Bug>
    {
        public BugRepository(BugTrackerDbContext context)
            : base(context)
        {
        }

        // custom query cho Bug nếu cần
    }
}