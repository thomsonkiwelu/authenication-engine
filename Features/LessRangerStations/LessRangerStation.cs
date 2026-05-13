using System.ComponentModel.DataAnnotations;
using conservation_backend.Features.LessOperationalZones;
using conservation_backend.Features.Offices;
using conservation_backend.Shared;

namespace conservation_backend.Features.LessRangerStations;

public class LessRangerStation : BaseEntity
{
    public Guid Id { get; set; }

    [MaxLength(150)]
    public string Name { get; set; } = string.Empty;

    [MaxLength(50)]
    public string? Code { get; set; }

    public Guid? LessOperationalZoneId { get; set; }

    public LessOperationalZone? LessOperationalZone { get; set; }

    public Guid? OfficeId { get; set; }

    public Office? Office { get; set; }
}
