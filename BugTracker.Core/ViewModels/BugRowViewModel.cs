using BugTracker.Core.Entities;
namespace BugTracker.Core.ViewModels
{
    public class BugRowViewModel
    {
        public Bug Bug { get; set; }

        public bool CanOpen { get; set; }
        public bool CanInProgress { get; set; }
        public bool CanFixed { get; set; }
        public bool CanClosed { get; set; }
    }
}    

