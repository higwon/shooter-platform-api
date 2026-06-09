using GamePlatform.Api.Application.DTOs;

namespace GamePlatform.Api.Application.Interfaces
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
