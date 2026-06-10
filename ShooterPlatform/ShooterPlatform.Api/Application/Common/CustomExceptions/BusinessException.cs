namespace ShooterPlatform.Api.Application.Common.CustomExceptions
{
    public class BusinessException : Exception
    {
        public BusinessException(string message) : base(message)
        {
        }
    }
}
