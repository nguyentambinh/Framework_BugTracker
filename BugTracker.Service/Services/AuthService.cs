using BugTracker.Core.Base;
using BugTracker.Core.Interfaces;
using BugTracker.Core.Dtos;

using BugTracker.Common.Helpers;

namespace BugTracker.Service.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepo;
        private readonly IUserContext _userContext;
        private readonly ISecurityLogger _securityLogger;

        public AuthService(
            IUserRepository userRepo,
            IUserContext userContext,
            ISecurityLogger securityLogger)
        {
            _userRepo = userRepo;
            _userContext = userContext;
            _securityLogger = securityLogger;
        }

        public AuthResult Login(LoginDto dto)
        {
            var user = _userRepo.GetByUsername(dto.Username);
            if (user == null)
            {
                _securityLogger.LogLoginFail(dto.Username, "User not found");
                return AuthResult.Fail("Sai tài khoản hoặc mật khẩu");
            }

            if (!PasswordHasher.Verify(dto.Password, user.PasswordHash))
            {
                _securityLogger.LogLoginFail(dto.Username, "Wrong Password");
                return AuthResult.Fail("Sai tài khoản hoặc mật khẩu");
            }

            if (!user.IsActive)
                return AuthResult.Fail("Tài khoản bị khóa");

            _userContext.SignIn(user.Id, user.UserName);
            _securityLogger.LogLoginSuccess(user.UserName);

            return AuthResult.Ok(user.Id, user.UserName);
        }
    }
}