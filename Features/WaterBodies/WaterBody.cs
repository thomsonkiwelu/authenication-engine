using System.ComponentModel.DataAnnotations;
using conservation_backend.Features.Parks;
using conservation_backend.Shared;

namespace conservation_backend.Features.WaterBodies;

public class WaterBody: BaseEntity
{
    public Guid Id { get; set; }

    [MaxLength(150)]
    public string Name { get; set; } = string.Empty;
    
    [MaxLength(150)]
    public string Type { get; set; } = string.Empty;
    
    public Guid ParkId { get; set; }
    public Park Park { get; set; } = null!;
}