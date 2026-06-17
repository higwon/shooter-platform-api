using ShooterPlatform.Api.Application.Features.Auth.DTOs;
using ShooterPlatform.Api.Application.Features.Auth.Models;

namespace ShooterPlatform.Api.Application.Features.Auth.Interfaces
{
    public interface IAuthService
    {
        string? Login(string username, string password);

        bool Register(string username, string password);

        List<AuthUserResponse> GetUsers();
    }
}
