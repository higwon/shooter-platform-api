using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using ShooterPlatform.Api.Application.Features.Analysis.DTOs;
using ShooterPlatform.Api.Application.Features.Analysis.Interfaces;
using System.Security.Claims;

namespace ShooterPlatform.Api.Application.Features.Analysis.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [EnableRateLimiting("analysis-limit")]
    [Authorize]
    public class ProfileAnalysisController : ControllerBase
    {
        private readonly IProfileAnalysisService _profileAnalysisService;
        private readonly IBatchProfileAnalysisService _batchProfileAnalysisService;
        private readonly IProfileCacheService _profileCacheService;

        public ProfileAnalysisController(
            IProfileAnalysisService profileAnalysisService,
            IBatchProfileAnalysisService batchProfileAnalysisService,
            IProfileCacheService profileCacheService)
        {
            _profileAnalysisService = profileAnalysisService;
            _batchProfileAnalysisService = batchProfileAnalysisService;
            _profileCacheService = profileCacheService;
        }

        // 단일 플레이어 분석
        [HttpGet("{battleTag}")]
        public async Task<IActionResult> Analyze(string battleTag)
        {
            var profile =
                await _profileCacheService.GetOrFetchAsync(battleTag);

            var result =
                await _profileAnalysisService.AnalyzeAsync(profile);

            return Ok(new ProfileAnalysisResponse
            {
                ProfileSummary = profile.Summary,
                Result = result
            });
        }

        // Favorite 기반 배치 분석
        [HttpGet("favorites")]
        public async Task<IActionResult> AnalyzeFavorites()
        {
            var userId = GetUserId();

            var result = await _batchProfileAnalysisService.AnalyzeFavoritesAsync(userId);

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