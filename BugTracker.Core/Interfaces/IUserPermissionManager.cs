using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BugTracker.Core.Interfaces
{
    public interface IUserPermissionManager
    {
        IUserPermissionManager ForUser(int userId);
        IUserPermissionManager SetAdmin();
        IUserPermissionManager SetDev();
        IUserPermissionManager SetTester();
        IUserPermissionManager SetRole(bool canOpen, bool canInProgress, bool canFixed, bool canClosed);
        void Save();
    }
}
