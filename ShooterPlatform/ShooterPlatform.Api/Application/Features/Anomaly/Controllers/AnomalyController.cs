using Microsoft.AspNetCore.Mvc;
using ShooterPlatform.Api.Application.Features.Anomaly.Interfaces;

namespace ShooterPlatform.Api.Application.Features.Anomaly.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AnomalyController : ControllerBase
    {
        private readonly IAnomalyService _anomalyService;

        public AnomalyController(IAnomalyService anomalyService)
        {
            _anomalyService = anomalyService;
        }

        [HttpGet("{battleTag}")]
        public async Task<IActionResult> Analyze(string battleTag)
        {
            var result = await _anomalyService.AnalyzeAsync(battleTag);

            return Ok(result);
        }
    }
}
