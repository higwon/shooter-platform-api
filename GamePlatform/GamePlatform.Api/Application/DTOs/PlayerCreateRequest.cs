using System.ComponentModel.DataAnnotations;

public class PlayerCreateRequest
{
    [Required]
    [MaxLength(20)]
    public string Name { get; set; }

    [Range(1, 100)]
    public int Level { get; set; }
}