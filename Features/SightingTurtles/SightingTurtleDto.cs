using conservation_backend.Features.Files;
using conservation_backend.Features.Locations;
using conservation_backend.Features.Parks;
using conservation_backend.Features.Users;
using conservation_backend.Shared;
using conservation_backend.Shared.Dtos;

namespace conservation_backend.Features.SightingTurtles;

public record SightingTurtlePaginationDto: PaginationDto
{
    public string? ParkId { get; set; } = string.Empty;
}

public record SightingTurtleDto
{
    public Guid Id { get; set; }
    public Guid LocalAreaNameId { get; set; }
    public Guid ParkId { get; set; }
    public int TotalHatchling { get; set; }
    public int TotalAdult { get; set; }
    public string Coordinates { get; set; } = string.Empty;
    public string Remark { get; set; } = string.Empty;
    public string CreatedBy { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public string UpdatedBy { get; set; } = string.Empty;
    public DateTime? UpdatedAt { get; set; }
    //Relationship
    public ParkDto Park { get; set; } = new ParkDto();
    public FileDto? File { get; set; } = new FileDto();
    public LocationDto Location { get; set; } = new LocationDto();
    public UserMinimalDto CreatedByUser { get; set; } = new UserMinimalDto();
    public UserMinimalDto UpdatedByUser { get; set; } = new UserMinimalDto();
}

public record SightingTurtleRequestDto
{
    public Guid LocalAreaNameId { get; set; }
    public string? Coordinates { get; set; } = string.Empty;
    public string Remark { get; set; } = string.Empty;
    public string? ImageId { get; set; } = string.Empty;
    public List<AnimalDemographicRequestDto> AnimalDemographics { get; set; } = new List<AnimalDemographicRequestDto>();
    public Guid? CreatedBy { get; set; }
    public Guid? UpdatedBy { get; set; }
}

public record AnimalDemographicRequestDto
{
    public string Category { get; set; } = string.Empty;
    public int NumberOfMale { get; set; }
    public int NumberOfFemale { get; set; }
}

public record SightingTurtleResponseDto : SightingTurtleDto
{
    public int RowNumber { get; set; }
}

public record GetSightingTurtleDto : SightingTurtleDto
{
    public List<AnimalDemographicDto> AnimalDemographics { get; set; } = new List<AnimalDemographicDto>();
}

public record SightingTurtleSqlResponseDto
{
    public List<SightingTurtleResponseDto> Data { get; set; } = new List<SightingTurtleResponseDto>();
    public PaginationMeta Meta { get; init; } = new ();
}