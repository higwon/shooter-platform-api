namespace GamePlatform.Api.Application.Features.Auth.Interfaces
{
    public interface IAuthService
    {
        string? Login(string username, string password);
    }
}
