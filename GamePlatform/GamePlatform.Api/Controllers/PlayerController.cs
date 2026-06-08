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

    [HttpPost]
    public ActionResult<Player> CreatePlayer(Player player)
    {
        _playerService.AddPlayer(player);

        return CreatedAtAction(
            nameof(GetPlayer),
            new { id = player.Id },
            player
        );
    }

    [HttpPut("{id}")]
    public ActionResult<Player> UpdatePlayer(int id, Player updatedPlayer)
    {
        var result = _playerService.UpdatePlayer(id, updatedPlayer);

        if (result == null)
            return NotFound();

        return Ok(result);
    }

    [HttpDelete("{id}")]
    public IActionResult DeletePlayer(int id)
    {
        var result = _playerService.DeletePlayer(id);

        if (!result)
            return NotFound();

        return NoContent();
    }
}