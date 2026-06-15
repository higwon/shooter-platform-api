using ShooterPlatform.Api.Application.Features.Analysis.Contexts;
using ShooterPlatform.Api.Application.Features.Analysis.Rules;
using ShooterPlatform.Api.Application.Features.Overwatch.Providers;

namespace ShooterPlatform.Tests.Features.Analysis.Rules
{
    public class AccuracyRuleTests
    {
        private readonly OverwatchProfileProvider _provider;
        private readonly AccuracyRule _rule;

        public AccuracyRuleTests()
        {
            _provider = new OverwatchProfileProvider(new HttpClient());
            _rule = new AccuracyRule();
        }

        [Fact]
        public async Task EvaluateAsync_ShouldAnalyzeRealProfile()
        {
            var battleTag =
                "c6718fa29a3cc9fcbfa025%7C3f7926bddbe82d5c9e0f2e7d181b2d2e";

            var profile = await _provider.FetchAsync(battleTag);

            var context = AnalysisTestHelper.CreateContext(profile);

            var result = await _rule.EvaluateAsync(context);

            Assert.NotNull(profile);

            if (result != null)
            {
                Assert.Equal("HIGH_ACCURACY", result.Code);
                Assert.NotEmpty(result.Message);
                Assert.True(result.Score > 0);
            }
        }
    }
}