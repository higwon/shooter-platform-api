using GamePlatform.Api.Application.Features.Auth.Interfaces;
using GamePlatform.Api.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace GamePlatform.Api.Application.Features.Auth.Services
{
    public class AuthService : IAuthService
    {
        private readonly JwtTokenService _jwtTokenService;
        private readonly GamePlatformDbContext _dbContext;


        public AuthService(JwtTokenService jwtTokenService, GamePlatformDbContext dbContext)
        {
            _jwtTokenService = jwtTokenService;
            _dbContext = dbContext;
        }

        public string? Login(string username, string password)
        {
            var user = _dbContext.Users
                .FirstOrDefault(x => x.Username == username && x.Password == password);

            if (user == null)
                return null;

            return _jwtTokenService.CreateToken(user);
        }
    }
}
