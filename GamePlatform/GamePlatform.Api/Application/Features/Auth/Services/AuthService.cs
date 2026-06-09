using GamePlatform.Api.Application.Features.Auth.Interfaces;
using GamePlatform.Api.Domain.Entities;
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
            var user = _dbContext.Users.FirstOrDefault(x => x.Username == username);

            if (user == null)
                return null;

            if (!BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
                return null;

            return _jwtTokenService.CreateToken(user);
        }

        public bool Register(string username, string password)
        {
            var exists = _dbContext.Users.Any(x => x.Username == username);

            if (exists)
                return false;

            var user = new User
            {
                Username = username,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(password)
            };

            _dbContext.Users.Add(user);
            _dbContext.SaveChanges();

            return true;
        }
    }
}
