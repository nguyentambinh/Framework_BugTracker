using System;

namespace BugTracker.Core.Exceptions
{
    // Exception cho lỗi nghiệp vụ
    public class BusinessException : Exception
    {
        // Mã lỗi nghiệp vụ
        public string ErrorCode { get; }

        // Có thể hiển thị cho người dùng không
        public bool IsUserFriendly { get; }

        public BusinessException(
            string message,
            string errorCode = null,
            bool isUserFriendly = true)
            : base(message)
        {
            ErrorCode = errorCode;
            IsUserFriendly = isUserFriendly;
        }

        public static BusinessException InvalidOperation(string message)
        {
            return new BusinessException(
                message,
                errorCode: "INVALID_OPERATION");
        }

        public static BusinessException PermissionDenied()
        {
            return new BusinessException(
                "Bạn không có quyền thực hiện thao tác này",
                errorCode: "PERMISSION_DENIED");
        }
    }
}