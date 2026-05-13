using conservation_backend.Features.LessOperationalZones;
using conservation_backend.Features.Offices;
using conservation_backend.Features.Users;
using conservation_backend.Shared;

namespace conservation_backend.Features.LessRangerStations;

public record LessRangerStationPaginationDto : PaginationDto
{
    public string? LessOperationalZoneId { get; init; }

    public string? OfficeId { get; init; }
}

public record LessRangerStationRequest(
    string Name,
    string? LessOperationalZoneId = null,
    string? OfficeId = null,
    string? Code = null
);

public record LessRangerStationDto
{
    public Guid Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string? Code { get; set; }

    public Guid? LessOperationalZoneId { get; set; }

    public LessOperationalZoneDto? LessOperationalZone { get; set; }

    public Guid? OfficeId { get; set; }

    public OfficeDto? Office { get; set; }

    public DateTime CreatedAt { get; set; }

    public string CreatedBy { get; set; } = string.Empty;

    public DateTime? UpdatedAt { get; set; }

    public string UpdatedBy { get; set; } = string.Empty;
}

public record LessRangerStationResponseDto : LessRangerStationDto
{
    public int RowNumber { get; set; }

    public UserMinimalDto Creator { get; set; } = new UserMinimalDto();
}
