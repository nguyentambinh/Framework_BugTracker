using System;

namespace BugTracker.Core.Base
{
    public abstract class BaseEntity
    {
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; }

        public bool IsDeleted { get; set; }
    }
}