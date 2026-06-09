using GamePlatform.Api.Application.Features.Auth.Interfaces;
using Microsoft.AspNetCore.Mvc;
using LoginRequest = GamePlatform.Api.Application.Features.Auth.DTOs.LoginRequest;

namespace GamePlatform.Api.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public IActionResult Login(LoginRequest req)
        {
            var token = _authService.Login(req.Username, req.Password);

            if (token == null)
                return Unauthorized();

            return Ok(token);
        }
    }
}
