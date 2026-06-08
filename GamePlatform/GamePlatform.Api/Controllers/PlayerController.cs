using GamePlatform.Api.Application.DTOs;
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
    public IActionResult CreatePlayer(PlayerDto dto)
    {
        _playerService.AddPlayer(dto);
        return Ok();
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