using GamePlatform.Api.Domain.Enums;

namespace GamePlatform.Api.Application.Features.Players.DTOs
{
    public class PlayerQueryRequest
    {
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 20;

        public string? Keyword { get; set; }
        public int? MinLevel { get; set; }
        public int? MaxLevel { get; set; }

        public PlayerSortBy SortBy { get; set; } = PlayerSortBy.Id;
        public SortOrder Order { get; set; } = SortOrder.Asc;
    }
}