using GamePlatform.Api.Application.Common.Enum;

namespace GamePlatform.Api.Application.DTOs
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