using conservation_backend.Features.Parks;
using conservation_backend.Features.Users;
using conservation_backend.Shared;

namespace conservation_backend.Features.LessOperationalZones;

public record LessOperationalZonePaginationDto : PaginationDto
{
    public string? ParkId { get; init; }
}

public record LessOperationalZoneRequest(
    string Name,
    string ParkId,
    string? Code
);

public record LessOperationalZoneDto
{
    public Guid Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string? Code { get; set; }

    public Guid ParkId { get; set; }

    public ParkDto Park { get; set; } = new ParkDto();

    public DateTime CreatedAt { get; set; }

    public string CreatedBy { get; set; } = string.Empty;

    public DateTime? UpdatedAt { get; set; }

    public string UpdatedBy { get; set; } = string.Empty;
}

public record LessOperationalZoneResponseDto : LessOperationalZoneDto
{
    public int RowNumber { get; set; }

    public UserMinimalDto Creator { get; set; } = new UserMinimalDto();
}
