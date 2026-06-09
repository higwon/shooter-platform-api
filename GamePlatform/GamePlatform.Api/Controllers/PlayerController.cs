using GamePlatform.Api.Application.Common;
using GamePlatform.Api.Application.Common.CustomExceptions;
using GamePlatform.Api.Application.Common.DTOs;
using GamePlatform.Api.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GamePlatform.Api.Controllers
{
    [ApiController]
    [Route("api/players")]
    public class PlayerController : ControllerBase
    {
        private readonly IPlayerService _playerService;

        public PlayerController(IPlayerService playerService)
        {
            _playerService = playerService;
        }

        [HttpGet]
        public ActionResult<ApiResult<IEnumerable<PlayerResponse>>> GetPlayers()
        {
            var players = _playerService.GetPlayers();

            return Ok(ApiResult<IEnumerable<PlayerResponse>>.Ok(players));
        }

        [HttpGet("{id}")]
        public ActionResult<ApiResult<PlayerResponse>> GetPlayer(int id)
        {
            var player = _playerService.GetPlayer(id);

            return Ok(ApiResult<PlayerResponse>.Ok(player));
        }


        [HttpPost]
        public ActionResult<ApiResult<string>> CreatePlayer(PlayerCreateRequest request)
        {
            if (!ModelState.IsValid)
                throw new BusinessException("Invalid request");

            _playerService.CreatePlayer(request);

            return Ok(ApiResult<string>.Ok("Created"));
        }

        [HttpPut("{id}")]
        public ActionResult<ApiResult<PlayerResponse>> UpdatePlayer(int id, PlayerUpdateRequest request)
        {
            var result = _playerService.UpdatePlayer(id, request);

            return Ok(ApiResult<PlayerResponse>.Ok(result));
        }

        [HttpDelete("{id}")]
        public ActionResult<ApiResult<string>> DeletePlayer(int id)
        {
            _playerService.DeletePlayer(id);

            return Ok(ApiResult<string>.Ok("Deleted"));
        }
    }
}

