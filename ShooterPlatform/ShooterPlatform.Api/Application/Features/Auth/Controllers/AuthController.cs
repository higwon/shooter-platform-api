using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using ShooterPlatform.Api.Application.Features.Auth.DTOs;
using ShooterPlatform.Api.Application.Features.Auth.Interfaces;
using LoginRequest = ShooterPlatform.Api.Application.Features.Auth.DTOs.LoginRequest;

namespace ShooterPlatform.Api.Application.Features.Auth.Controllers
{
    [ApiController]
    [Route("api/auth")]
    [EnableRateLimiting("auth-limit")]
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

        [HttpPost("register")]
        public IActionResult Register(RegisterRequest req)
        {
            var result = _authService.Register(req.Username, req.Password);

            if (!result)
                return BadRequest("User already exists");

            return Ok();
        }

        [HttpGet("users")]
        public IActionResult GetUsers()
        {
            var users = _authService.GetUsers();
            return Ok(users);
        }
    }
}
