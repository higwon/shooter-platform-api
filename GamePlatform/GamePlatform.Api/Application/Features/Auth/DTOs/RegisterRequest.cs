using System.ComponentModel.DataAnnotations;

namespace GamePlatform.Api.Application.Features.Auth.DTOs
{
    public class RegisterRequest
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}