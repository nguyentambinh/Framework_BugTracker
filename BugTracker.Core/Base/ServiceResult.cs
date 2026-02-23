namespace BugTracker.Core.Base
{
    public class ServiceResult
    {
        //Chuẩn hóa kết quả trả về
        public bool Success { get; protected set; }
        public string Message { get; protected set; }

        public static ServiceResult Ok(string message = "")
            => new ServiceResult { Success = true, Message = message };

        public static ServiceResult Fail(string message)
            => new ServiceResult { Success = false, Message = message };
    }
    public class ServiceResult<T> : ServiceResult
    {
        public T Data { get; private set; }

        public static ServiceResult<T> Ok(T data, string message = "")
            => new ServiceResult<T>
            {
                Success = true,
                Message = message,
                Data = data
            };

        public new static ServiceResult<T> Fail(string message)
            => new ServiceResult<T>
            {
                Success = false,
                Message = message,
                Data = default
            };
    }
}