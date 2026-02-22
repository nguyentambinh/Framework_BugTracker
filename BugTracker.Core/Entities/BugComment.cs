using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BugTracker.Core.Base;
namespace BugTracker.Core.Entities
{
    //Lưu lịch sử của Bug
    public class BugComment : BaseEntity
    {
        public int BugId { get; set; }

        public string Comment { get; set; }

        public int CreatedByUserId { get; set; }
    }
}
