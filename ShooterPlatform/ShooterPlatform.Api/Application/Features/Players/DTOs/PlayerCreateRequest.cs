using System.ComponentModel.DataAnnotations;

namespace ShooterPlatform.Api.Application.Features.Players.DTOs
{
    public class PlayerCreateRequest
    {
        public string Name { get; set; }

        public int Level { get; set; }
    }
}
