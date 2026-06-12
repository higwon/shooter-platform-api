using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShooterPlatform.Api.Application.Features.Anomaly.DTOs;
using ShooterPlatform.Api.Application.Features.Anomaly.Interfaces;
using ShooterPlatform.Api.Application.Features.Anomaly.Services;
using System.Security.Claims;

namespace ShooterPlatform.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class AnomalyController : ControllerBase
    {
        private readonly IAnomalyService _anomalyService;
        private readonly IBatchAnomalyService _batchAnomalyService;
        private readonly IProfileCacheService _profileCacheService;

        public AnomalyController(
            IAnomalyService anomalyService,
            IBatchAnomalyService batchAnomalyService,
            IProfileCacheService profileCacheService)
        {
            _anomalyService = anomalyService;
            _batchAnomalyService = batchAnomalyService;
            _profileCacheService = profileCacheService;
        }

        // 단일 플레이어 분석
        [HttpGet("{battleTag}")]
        public async Task<IActionResult> Analyze(string battleTag)
        {
            var profile =
                await _profileCacheService.GetOrFetchAsync(battleTag);

            var result =
                await _anomalyService.AnalyzeAsync(profile);

            return Ok(new AnomalyResponse
            {
                Profile = profile,
                Result = result
            });
        }

        // Favorite 기반 배치 분석
        [HttpGet("favorites")]
        public async Task<IActionResult> AnalyzeFavorites()
        {
            var userId = GetUserId();

            var result = await _batchAnomalyService.AnalyzeFavoritesAsync(userId);

            return Ok(result);
        }

        // JWT에서 userId 추출
        private int GetUserId()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (!int.TryParse(userIdClaim, out var userId))
                throw new UnauthorizedAccessException("Invalid user.");

            return userId;
        }
    }
}