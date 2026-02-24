using BugTracker.Core.Entities;
using System.Collections.Generic;

namespace BugTracker.Core.ViewModels
{
    public class BugDashboardViewModel
    {
        public List<BugRowViewModel> Bugs { get; set; }
        public List<BugGroup> BugGroups { get; set; }
        public List<UserPermissionViewModel> Users { get; set; }
        public List<Role> AllRoles { get; set; }
        public int TotalBugs { get; set; }
        public int OpenBugs { get; set; }
        public int InProgressBugs { get; set; }
        public int ClosedBugs { get; set; }

        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public bool IsAdmin { get; set; }
    }

    public class UserPermissionViewModel
    {
        public int Id { get; set; }
        public string DisplayName { get; set; }
        public bool CanOpen { get; set; }
        public bool CanInProgress { get; set; }
        public bool CanFixed { get; set; }
        public bool CanClosed { get; set; }
    }
}