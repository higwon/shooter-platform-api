using GamePlatform.Api.Models;
using Microsoft.AspNetCore.Mvc;

namespace GamePlatform.Api.Controllers
{
    [ApiController]
    [Route("api/players")]
    public class PlayerController : ControllerBase
    {
        [HttpGet]
        public IEnumerable<Player> GetPlayers()
        {
            return
            [
                new Player
            {
                Id = 1,
                Name = "HyuckJin",
                Level = 100,
                CreatedAt = DateTime.UtcNow
            }
            ];
        }
    }
}
