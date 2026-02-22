using BugTracker.Core.Enums;

namespace BugTracker.Core.Workflow
{
    public static class BugWorkflow
    {
        public static bool CanChangeStatus(BugStatus from, BugStatus to)
        {
            if (from == BugStatus.Closed)
                return false;

            if (from == BugStatus.Open && to == BugStatus.Fixed)
                return false;

            return true;
        }
    }
}