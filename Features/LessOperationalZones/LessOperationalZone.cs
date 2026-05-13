using System.ComponentModel.DataAnnotations;
using authentication_engine.Features.Parks;
using authentication_engine.Shared.Entities;

namespace authentication_engine.Features.LessOperationalZones;

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
