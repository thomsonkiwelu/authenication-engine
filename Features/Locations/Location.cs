using System.ComponentModel.DataAnnotations;
using conservation_backend.Features.Parks;
using conservation_backend.Shared;

namespace conservation_backend.Features.Locations;

public class Location : BaseEntity
{
    public Guid Id { get; set; }

    [MaxLength(255)]
    public string Name { get; set; } = string.Empty;
    
    public Guid ParkId { get; set; }
    
    public Park Park { get; set; } = null!;
}