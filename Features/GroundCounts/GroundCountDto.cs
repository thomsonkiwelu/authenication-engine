using conservation_backend.Features.Files;
using conservation_backend.Features.Locations;
using conservation_backend.Features.Parks;
using conservation_backend.Features.Species;
using conservation_backend.Features.Users;
using conservation_backend.Shared;
using conservation_backend.Shared.Dtos;

namespace conservation_backend.Features.GroundCounts;

public record GroundCountPaginationDto : PaginationDto
{
    public string? ParkId { get; set; }
    public string? Method { get; set; }
}

public record GroundCountDto
{
    public Guid Id { get; set; }
    public Guid LocalAreaNameId { get; set; }
    public Guid ParkId { get; set; }
    public string TransectId { get; set; } = string.Empty;
    public string Method { get; set; } = string.Empty;
    public string TransectStartingPoint { get; set; } = string.Empty;
    public string TransectEndPoint { get; set; } = string.Empty;
    public float EndDistance { get; set; }
    public string RouteDescription { get; set; } = string.Empty;
    public string WeatherCondition { get; set; } = string.Empty;
    public string OtherWeatherCondition { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public string CreatedBy { get; set; } = string.Empty;
    public DateTime? UpdatedAt { get; set; }
    public string UpdatedBy { get; set; } = string.Empty;
    //Relationship
    public ParkDto Park { get; set; } = new ParkDto();
    public LocationDto Location { get; set; } = new LocationDto();
    public UserMinimalDto Creator { get; set; } = new UserMinimalDto();
}

public record GroundCountSightingDto
{
    public Guid Id { get; set; }
    public Guid SpeciesId { get; set; }
    public Guid GroundCountId { get; set; }
    public string TimeOfSighting { get; set; } = string.Empty;
    public float PerpendicularDistance { get; set; }
    public float Distance { get; set; }
    public string Remark { get; set; } = string.Empty;
    public string? Coordinates { get; set; } = string.Empty;
    public string CreatedBy { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public string UpdatedBy { get; set; } = string.Empty;
    public DateTime? UpdatedAt { get; set; }
    //Relationship
    public ICollection<AnimalDemographicDto> AnimalDemographics { get; set; } = new List<AnimalDemographicDto>();
    public SpeciesDto Species { get; set; } = new SpeciesDto();
    public FileDto? File { get; set; } = new FileDto();
}

public record GroundCountResponseDto : GroundCountDto
{
    public int RowNumber { get; set; }
}

public record GetGroundCountDto : GroundCountDto
{
    public List<GroundCountSightingDto> GroundCountSightings { get; set; } = new List<GroundCountSightingDto>();
    public UserMinimalDto CreatedByUser { get; set; } = new UserMinimalDto();
    public UserMinimalDto UpdatedByUser { get; set; } = new UserMinimalDto();
}

public record GroundCountRequestDto
{
    public Guid LocalAreaNameId { get; set; }
    public string TransectId { get; set; } = string.Empty;
    public string Method { get; set; } = string.Empty;
    public string TransectStartingPoint { get; set; } = string.Empty;
    public string TransectEndPoint { get; set; } = string.Empty;
    public float EndDistance { get; set; }
    public string RouteDescription { get; set; } = string.Empty;
    public string WeatherCondition { get; set; } = string.Empty;
    public string OtherWeatherCondition { get; set; } = string.Empty;
    public List<GroundCountSightingRequestDto> AnimalObserved { get; set; } = new List<GroundCountSightingRequestDto>();
    public Guid? CreatedBy { get; set; }
    public Guid? UpdatedBy { get; set; }
}

public record GroundCountSightingRequestDto
{
    public Guid? Id { get; set; }
    public Guid SpeciesId { get; set; }
    public string TimeOfSighting { get; set; } = string.Empty;
    public float PerpendicularDistance { get; set; }
    public float Distance { get; set; }
    public string Remark { get; set; } = string.Empty;
    public string? Coordinates { get; set; } = string.Empty;
    public string? ImageId { get; set; } = string.Empty;
    public List<AnimalCountRequestDto> AnimalCount { get; set; } = new List<AnimalCountRequestDto>();
}

public record AnimalCountRequestDto
{
    public string Age { get; set; } = string.Empty;
    public string Sex { get; set; } = string.Empty;
    public float Count { get; set; }
}