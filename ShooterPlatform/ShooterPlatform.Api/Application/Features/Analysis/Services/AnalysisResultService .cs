using Microsoft.EntityFrameworkCore;
using ShooterPlatform.Api.Application.Features.Analysis.DTOs;
using ShooterPlatform.Api.Application.Features.Analysis.Interfaces;
using ShooterPlatform.Api.Application.Features.Analysis.Models;
using ShooterPlatform.Api.Infrastructure;
using System.Text.Json;

namespace ShooterPlatform.Api.Application.Features.Analysis.Services
{
    public class AnalysisResultService : IAnalysisResultService
    {
        private readonly ShooterPlatformDbContext _db;

        public AnalysisResultService(ShooterPlatformDbContext db)
        {
            _db = db;
        }

        public async Task<AnalysisResult?> GetByBattleTagAsync(string battleTag)
        {
            return await _db.AnalysisResults.FirstOrDefaultAsync(x => x.BattleTag == battleTag);
        }

        public async Task<List<AnalysisResult>> GetByBattleTagsAsync(IEnumerable<string> battleTags)
        {
            return await _db.AnalysisResults
                .Where(x => battleTags.Contains(x.BattleTag))
                .ToListAsync();
        }

        public async Task SaveOrUpdateAsync(string battleTag, ProfileAnalysisResult result)
        {
            var entity = await _db.AnalysisResults
                .FirstOrDefaultAsync(x => x.BattleTag == battleTag);

            var flagsJson = JsonSerializer.Serialize(result.Flags);

            if (entity == null)
            {
                entity = new AnalysisResult
                {
                    BattleTag = battleTag,
                    RiskScore = result.RiskScore,
                    RiskLevel = result.RiskLevel,
                    FlagsJson = flagsJson,
                    AnalyzedAt = DateTime.UtcNow
                };

                _db.AnalysisResults.Add(entity);
            }
            else
            {
                entity.RiskScore = result.RiskScore;
                entity.RiskLevel = result.RiskLevel;
                entity.FlagsJson = flagsJson;
                entity.AnalyzedAt = DateTime.UtcNow;
            }

            await _db.SaveChangesAsync();
        }
    }
}
