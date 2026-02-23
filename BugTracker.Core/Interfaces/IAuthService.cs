using BugTracker.Core.Base;
using BugTracker.Core.Dtos;

namespace BugTracker.Core.Interfaces
{
    public interface IAuthService
    {
        AuthResult Login(LoginDto dto);
    }
}