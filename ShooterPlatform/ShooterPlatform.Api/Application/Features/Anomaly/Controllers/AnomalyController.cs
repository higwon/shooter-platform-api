using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShooterPlatform.Api.Application.Features.Anomaly.Interfaces;
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

        public AnomalyController(
            IAnomalyService anomalyService,
            IBatchAnomalyService batchAnomalyService)
        {
            _anomalyService = anomalyService;
            _batchAnomalyService = batchAnomalyService;
        }

        // 단일 플레이어 분석
        [HttpGet("{battleTag}")]
        public async Task<IActionResult> Analyze(string battleTag)
        {
            var result = await _anomalyService.AnalyzeAsync(battleTag);

            return Ok(result);
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