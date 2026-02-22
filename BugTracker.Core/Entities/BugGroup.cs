using System.Collections.Generic;
using BugTracker.Core.Base;

namespace BugTracker.Core.Entities
{
    public class BugGroup : BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }

        public virtual ICollection<Bug> Bugs { get; set; }
    }
}