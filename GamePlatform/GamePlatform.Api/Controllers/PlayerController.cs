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
    public IEnumerable<Player> GetPlayers()
    {
        return _playerService.GetPlayers();
    }
}