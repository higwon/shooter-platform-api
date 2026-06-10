using ShooterPlatform.Api.Application.Features.Auth.Interfaces;
using ShooterPlatform.Api.Domain.Entities;
using ShooterPlatform.Api.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace ShooterPlatform.Api.Application.Features.Auth.Services
{
    public class AuthService : IAuthService
    {
        private readonly JwtTokenService _jwtTokenService;
        private readonly ShooterPlatformDbContext _dbContext;


        public AuthService(JwtTokenService jwtTokenService, ShooterPlatformDbContext dbContext)
        {
            _jwtTokenService = jwtTokenService;
            _dbContext = dbContext;
        }

        public string? Login(string email, string password)
        {
            var user = _dbContext.Users.FirstOrDefault(x => x.Email == email);

            if (user == null)
                return null;

            if (!BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
                return null;

            return _jwtTokenService.CreateToken(user);
        }

        public bool Register(string email, string password)
        {
            var exists = _dbContext.Users.Any(x => x.Email == email);

            if (exists)
                return false;

            var user = new User
            {
                Email = email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(password)
            };

            _dbContext.Users.Add(user);
            _dbContext.SaveChanges();

            return true;
        }
    }
}
