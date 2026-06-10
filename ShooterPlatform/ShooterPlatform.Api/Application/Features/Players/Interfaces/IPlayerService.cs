using ShooterPlatform.Api.Application.Features.Players.DTOs;

namespace ShooterPlatform.Api.Application.Features.Players.Interfaces
{
    public interface IPlayerService
    {
        IEnumerable<PlayerResponse> GetAllPlayers();

        PagedResult<PlayerResponse> GetPlayers(PlayerQueryRequest request);

        PlayerResponse? GetPlayer(int id);

        void CreatePlayer(PlayerCreateRequest request);

        PlayerResponse? UpdatePlayer(int id, PlayerUpdateRequest player);

        void DeletePlayer(int id);
    }
}
