using System.Collections.Generic;
using BugTracker.Core.Base;

namespace BugTracker.Core.Entities
{
    public class ApplicationUser : BaseEntity
    {
        public string UserName { get; set; }
        public string DisplayName { get; set; }
        public string Role { get; set; }
        public bool IsActive { get; set; }

        // navigation
        public virtual ICollection<Bug> AssignedBugs { get; set; }
        public virtual ICollection<Bug> CreatedBugs { get; set; }
    }
}