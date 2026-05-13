using System.ComponentModel.DataAnnotations;
using authentication_engine.Features.LessOperationalZones;
using authentication_engine.Features.Offices;
using authentication_engine.Shared.Entities;

namespace authentication_engine.Features.LessRangerStations;

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
