using System;
using BugTracker.Core.Base;
using BugTracker.Core.Enums;

namespace BugTracker.Core.Entities
{
    public class Bug : BaseEntity, IAuditable
    {
        public string Title { get; set; }
        public string Description { get; set; }

        public BugPriority Priority { get; set; }
        public BugStatus Status { get; set; }
        public DateTime CreatedDate { get; set; }
        // FK - Group
        public int? BugGroupId { get; set; }
        public virtual BugGroup BugGroup { get; set; }

        // FK - User (INT)
        public int CreatedByUserId { get; set; }
        public virtual ApplicationUser CreatedByUser { get; set; }

        public int? AssignedToUserId { get; set; }
        public virtual ApplicationUser AssignedToUser { get; set; }
    }
}