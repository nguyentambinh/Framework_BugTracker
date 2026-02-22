using System.Collections.Generic;

namespace BugTracker.Common.Models
{
    public class PagedResult<T>
    {
        public int TotalItems { get; set; }
        public List<T> Items { get; set; }

        public PagedResult()
        {
            Items = new List<T>();
        }
    }
}