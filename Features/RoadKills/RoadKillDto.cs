using conservation_backend.Features.Files;
using conservation_backend.Features.Locations;
using conservation_backend.Features.Parks;
using conservation_backend.Features.Species;
using conservation_backend.Features.Users;
using conservation_backend.Shared;
using conservation_backend.Shared.Dtos;

namespace conservation_backend.Features.RoadKills;

public record RoadKillPaginationDto: PaginationDto
{
    public string? ParkId { get; set; } = string.Empty;
}

public record RoadKillDto
{
    public Guid Id { get; set; }
    public Guid SpeciesId { get; set; }
    public Guid LocalAreaNameId { get; set; }
    public Guid ParkId { get; set; }
    public string Coordinates { get; set; } = string.Empty;
    public string Remark { get; set; } = string.Empty;
    public string CreatedBy { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public string UpdatedBy { get; set; } = string.Empty;
    public DateTime? UpdatedAt { get; set; }
    public int TotalAnimalCount { get; set; }
    //Relationship
    public ParkDto Park { get; set; } = new ParkDto();
    public FileDto? File { get; set; } = new FileDto();
    public SpeciesDto Species { get; set; } = new SpeciesDto();
    public LocationDto Location { get; set; } = new LocationDto();
    public UserMinimalDto CreatedByUser { get; set; } = new UserMinimalDto();
    public UserMinimalDto UpdatedByUser { get; set; } = new UserMinimalDto();
}

public record RoadKillRequestDto
{
    public Guid SpeciesId { get; set; }
    public Guid LocalAreaNameId { get; set; }
    public string? Coordinates { get; set; } = string.Empty;
    public string Remark { get; set; } = string.Empty;
    public string? ImageId { get; set; } = string.Empty;
    public List<AnimalKilledDto> AnimalKilled { get; set; } = new List<AnimalKilledDto>();
    public Guid? CreatedBy { get; set; }
    public Guid? UpdatedBy { get; set; }
}

public record AnimalKilledDto
{
    public string Age { get; set; } = string.Empty;
    public string Sex { get; set; } = string.Empty;
    public int Count { get; set; }
}

public record RoadKillResponseDto : RoadKillDto
{
    public int RowNumber { get; set; }
}

public record RoadKillSqlResponseDto
{
    public List<RoadKillResponseDto> Data { get; set; } = new();
    public PaginationMeta Meta { get; init; } = new ();
}

public record GetRoadKillDto : RoadKillDto
{
    public List<AnimalDemographicDto> AnimalDemographics { get; set; } = new List<AnimalDemographicDto>();
}