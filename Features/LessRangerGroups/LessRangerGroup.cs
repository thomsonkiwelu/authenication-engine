using System.ComponentModel.DataAnnotations;
using conservation_backend.Features.LessRangerStations;
using conservation_backend.Shared;

namespace conservation_backend.Features.LessRangerGroups;

public class LessRangerGroup : BaseEntity
{
    public Guid Id { get; set; }

    [MaxLength(150)]
    public string Name { get; set; } = string.Empty;

    [MaxLength(50)]
    public string? Code { get; set; }

    public Guid LessRangerStationId { get; set; }

    public LessRangerStation LessRangerStation { get; set; } = null!;
}
