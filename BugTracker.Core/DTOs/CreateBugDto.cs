using BugTracker.Core.Enums;
using System.ComponentModel.DataAnnotations;

namespace BugTracker.Core.DTOs
{
    public class CreateBugDto
    {
        [Required]
        [StringLength(200)]
        public string Title { get; set; }

        public string Description { get; set; }

        [Required]
        public BugPriority Priority { get; set; }

        public int? BugGroupId { get; set; }

        public int? AssignedToUserId { get; set; }
    }
}