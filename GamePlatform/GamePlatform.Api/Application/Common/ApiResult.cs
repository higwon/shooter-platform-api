namespace GamePlatform.Api.Application.Common
{
    public class ApiResult<T>
    {
        public bool Success { get; init; }
        public T? Data { get; init; }
        public string? Message { get; init; }
        public string? ErrorCode { get; init; }
        public object? Errors { get; init; }

        public static ApiResult<T> Ok(T data, string? message = null)
        {
            return new ApiResult<T>
            {
                Success = true,
                Data = data,
                Message = message
            };
        }
    }
}