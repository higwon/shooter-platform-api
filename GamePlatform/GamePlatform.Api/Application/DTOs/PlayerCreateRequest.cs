using System.ComponentModel.DataAnnotations;

namespace GamePlatform.Api.Application.DTOs
{
    public class PlayerCreateRequest
    {
        public string Name { get; set; }

        public int Level { get; set; }
    }
}
