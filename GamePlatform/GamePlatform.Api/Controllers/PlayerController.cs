using GamePlatform.Api.Application.Common;
using GamePlatform.Api.Application.Interfaces;
using GamePlatform.Api.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace GamePlatform.Api.Controllers;

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
    public ActionResult<IEnumerable<PlayerResponse>> GetPlayers()
    {
        var players = _playerService.GetPlayers();

        return Ok(ApiResult<IEnumerable<PlayerResponse>>.Ok(players));
    }

    [HttpGet("{id}")]
    public ActionResult<ApiResult<PlayerResponse>> GetPlayer(int id)
    {
        var player = _playerService.GetPlayer(id);

        if (player is null)
            return Ok(ApiResult<PlayerResponse>.Fail("Player not found"));

        return Ok(ApiResult<PlayerResponse>.Ok(player));
    }


    [HttpPost]
    public IActionResult CreatePlayer(PlayerCreateRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ApiResult<string>.Fail("Invalid request"));

        _playerService.CreatePlayer(request);

        return Ok(ApiResult<string>.Ok("Created"));
    }

    [HttpPut("{id}")]
    public ActionResult<Player> UpdatePlayer(int id, PlayerUpdateRequest updatedPlayer)
    {
        var result = _playerService.UpdatePlayer(id, updatedPlayer);

        if (result == null)
            return Ok(ApiResult<PlayerResponse>.Fail("Not found"));

        return Ok(ApiResult<PlayerResponse>.Ok(result));
    }

    [HttpDelete("{id}")]
    public IActionResult DeletePlayer(int id)
    {
        var result = _playerService.DeletePlayer(id);

        if (!result)
            return Ok(ApiResult<string>.Fail("Not found"));

        return Ok(ApiResult<string>.Ok("Deleted"));
    }
}