using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BugTracker.Core.Interfaces;

namespace BugTracker.Service.Services
{
    public class SecurityLogger : ISecurityLogger
    {
        public SecurityLogger()
        {
        }

        public void LogLoginSuccess(string username)
        {
            Debug.WriteLine($"[SECURITY] Login success: {username}");
        }

        public void LogLoginFail(string username, string reason)
        {
            Debug.WriteLine($"[SECURITY] Login failed: {username} - {reason}");
        }
    }
}
