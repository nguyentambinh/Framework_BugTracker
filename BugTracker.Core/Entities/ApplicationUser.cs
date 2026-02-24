using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using BugTracker.Core.Base;

namespace BugTracker.Core.Entities
{
    public class ApplicationUser : BaseEntity
    {
        public string UserName { get; set; }
        public string DisplayName { get; set; }
        public int? RoleId { get; set; } // nullable
        public virtual Role Role { get; set; }
        public string PasswordHash { get; set; }
        public bool IsActive { get; set; }

        // navigation
        public virtual ICollection<Bug> AssignedBugs { get; set; 
        }
        public virtual ICollection<Bug> CreatedBugs { get; set; }
        [NotMapped] public bool CanOpen { get; set; }
        [NotMapped] public bool CanInProgress { get; set; }
        [NotMapped] public bool CanFixed { get; set; }
        [NotMapped] public bool CanClosed { get; set; }
    }
}