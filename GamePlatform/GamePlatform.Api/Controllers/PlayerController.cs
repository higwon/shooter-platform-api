using GamePlatform.Api.Models;
using GamePlatform.Api.Services;
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
    public ActionResult<IEnumerable<Player>> GetPlayers()
    {
        var players = _playerService.GetPlayers();

        return Ok(players);
    }

    [HttpGet("{id}")]
    public ActionResult<Player> GetPlayer(int id)
    {
        var player = _playerService.GetPlayer(id);

        if (player is null)
        {
            return NotFound();
        }

        return Ok(player);
    }
}