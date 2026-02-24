using System;

namespace BugTracker.Core.Entities
{
    public class AuditLog
    {
        public int Id { get; set; }

        public string EntityName { get; set; }
        public int EntityId { get; set; }

        public string Action { get; set; }   
        public string Changes { get; set; } 

        public int? UserId { get; set; }
        public string UserName { get; set; }

        public string IpAddress { get; set; }

        public DateTime ChangedAt { get; set; }  
        public DateTime CreatedAt { get; set; }  
    }
}