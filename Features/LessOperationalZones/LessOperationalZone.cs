using System.ComponentModel.DataAnnotations;
using conservation_backend.Features.Parks;
using conservation_backend.Shared;

namespace conservation_backend.Features.LessOperationalZones;

public class LessOperationalZone : BaseEntity
{
    public Guid Id { get; set; }

    [MaxLength(150)]
    public string Name { get; set; } = string.Empty;

    [MaxLength(50)]
    public string? Code { get; set; }

    public Guid ParkId { get; set; }

    public Park Park { get; set; } = null!;
}
