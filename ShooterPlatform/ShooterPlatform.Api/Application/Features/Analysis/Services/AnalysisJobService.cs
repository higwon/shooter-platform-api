using ShooterPlatform.Api.Application.Features.Analysis.Interfaces;
using ShooterPlatform.Api.Infrastructure;
using Microsoft.EntityFrameworkCore;

public class AnalysisJobService : IAnalysisJobService
{
    private readonly ShooterPlatformDbContext _dbContext;
    private readonly IAnalysisRefreshService _analysisRefreshService;
    private readonly ILogger<AnalysisJobService> _logger;

    public AnalysisJobService(
        ShooterPlatformDbContext dbContext,
        IAnalysisRefreshService analysisRefreshService,
        ILogger<AnalysisJobService> logger)
    {
        _dbContext = dbContext;
        _analysisRefreshService = analysisRefreshService;
        _logger = logger;
    }

    public async Task RefreshAllAsync()
    {
        var battleTags = await _dbContext.FavoritePlayers
            .Select(x => x.BattleTag)
            .Distinct()
            .ToListAsync();

        foreach (var battleTag in battleTags)
        {
            try
            {
                await _analysisRefreshService
                    .AnalyzeAndSaveAsync(battleTag);
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Failed to refresh analysis for {BattleTag}",
                    battleTag);
            }
        }
    }
}