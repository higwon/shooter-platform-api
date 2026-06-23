using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using ShooterPlatform.Api.Application.Features.Analysis.Interfaces;
using ShooterPlatform.Api.Application.Features.Favorite.DTOs;
using ShooterPlatform.Api.Application.Features.Favorite.Interfaces;
using System.Security.Claims;

namespace ShooterPlatform.Api.Application.Features.Favorite.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [EnableRateLimiting("profile-limit")]
    [Authorize]
    public class FavoritesController : ControllerBase
    {
        private readonly IFavoriteService _favoriteService;
        private readonly IAnalysisRefreshService _analysisRefreshService;

        public FavoritesController(IFavoriteService favoriteService, 
            IAnalysisRefreshService analysisRefreshService)
        {
            _favoriteService = favoriteService;
            _analysisRefreshService = analysisRefreshService;
        }

        [HttpPost]
        public async Task<IActionResult> Add( [FromBody] AddFavoriteRequest request)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (!int.TryParse(userIdClaim, out var userId))
                return Unauthorized();

            var favorite =
                await _favoriteService.AddFavoriteAsync(
                    userId,
                    request.BattleTag);

            await _analysisRefreshService
                .AnalyzeAndSaveAsync(
                    request.BattleTag);

            return Ok(favorite);
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (!int.TryParse(userIdClaim, out var userId))
                return Unauthorized();

            await _favoriteService
                .RefreshAnalysisAsync(userId);

            return NoContent();
        }

        [HttpGet]
        public async Task<IActionResult> GetFavorites()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (!int.TryParse(userIdClaim, out var userId))
                return Unauthorized();

            var favorites = await _favoriteService.GetFavoritesAsync(userId);

            return Ok(favorites);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (!int.TryParse(userIdClaim, out var userId))
                return Unauthorized();

            await _favoriteService.DeleteFavoriteAsync(userId, id);

            return NoContent();
        }
    }
}

