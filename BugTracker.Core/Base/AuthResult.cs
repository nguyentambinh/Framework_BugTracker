namespace BugTracker.Core.Base
{
    public class AuthResult : ServiceResult
    {
        public int? UserId { get; private set; }
        public string UserName { get; private set; }

        public static AuthResult Ok(int userId, string userName)
        {
            return new AuthResult
            {
                Success = true,
                UserId = userId,
                UserName = userName
            };
        }

        public new static AuthResult Fail(string message)
        {
            return new AuthResult
            {
                Success = false,
                Message = message
            };
        }
    }
}