using System.ComponentModel.DataAnnotations;

namespace ShooterPlatform.Api.Application.Features.Auth.DTOs
{
    public class LoginRequest
    {
        [Required]
        public string Username { get; set; }
        
        [Required]
        public string Password { get; set; }
    }
}