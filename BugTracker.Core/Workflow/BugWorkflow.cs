using BugTracker.Core.Enums;

namespace BugTracker.Core.Workflow
{
    public static class BugWorkflow
    {
        public static (bool IsValid, string Message) CanTransition(BugStatus currentStatus, BugStatus newStatus)
        {
            if (currentStatus == newStatus)
                return (true, string.Empty);

            if (currentStatus == BugStatus.Closed)
                return (false, "Bug đã đóng thì không thể thay đổi trạng thái nữa.");

            if (currentStatus == BugStatus.Open && newStatus == BugStatus.Fixed)
                return (false, "Không thể nhảy vọt từ Open sang Fixed mà không qua InProgress.");

            return (true, string.Empty);
        }
    }
}