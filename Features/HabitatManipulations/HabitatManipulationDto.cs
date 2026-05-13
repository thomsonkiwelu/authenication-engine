using conservation_backend.Features.Locations;
using conservation_backend.Features.Parks;
using conservation_backend.Features.Species;
using conservation_backend.Features.Users;
using conservation_backend.Shared;
using conservation_backend.Shared.Dtos;

namespace conservation_backend.Features.HabitatManipulations;

public record HabitatManipulationPaginationDto : PaginationDto
{
    public string? ActionTaken { get; set; }
    public string? ParkId { get; set; }
}
    
public record HabitatManipulationDto
{
    public Guid Id { get; set; }
    public Guid LocalAreaNameId { get; set; }
    public Guid ParkId { get; set; }
    public Guid SpeciesId { get; set; }
    public string Session { get; set; } = string.Empty;
    public string Environment { get; set; } = string.Empty;
    public float AreaCovered { get; set; }
    public string ActionTaken { get; set; } = string.Empty;
    public float TotalNumber { get; set; }
    public string? SourceOfSeedling { get; set; } = string.Empty;
    public string? Coordinates { get; set; } = string.Empty;
    public string ExpectedOutput { get; set; } = string.Empty;
    public string Remark { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public string CreatedBy { get; set; } = string.Empty;
    public DateTime? UpdatedAt { get; set; }
    public string UpdatedBy { get; set; } = string.Empty;
    //Relationship
    public ParkDto Park { get; set; } = new ParkDto();
    public SpeciesDto Species { get; set; } = new SpeciesDto();
    public LocationDto Location { get; set; } = new LocationDto();
    public UserMinimalDto CreatedByUser { get; set; } = new UserMinimalDto();
    public UserMinimalDto UpdatedByUser { get; set; } = new UserMinimalDto();
}

public record HabitatManipulationResponseDto : HabitatManipulationDto
{
    public int RowNumber { get; set; }
}

public record HabitatManipulationSqlResponseDto
{
    public List<HabitatManipulationResponseDto> Data { get; set; } = new();
    public PaginationMeta Meta { get; init; } = new ();
}

public record HabitatManipulationRequestDto
{
    public Guid LocalAreaNameId { get; set; }
    public Guid ParkId { get; set; }
    public Guid SpeciesId { get; set; }
    public string Session { get; set; } = string.Empty;
    public string Environment { get; set; } = string.Empty;
    public float AreaCovered { get; set; }
    public string ActionTaken { get; set; } = string.Empty;
    public float TotalNumber { get; set; }
    public string? SourceOfSeedling { get; set; } = string.Empty;
    public string? Coordinates { get; set; } = string.Empty;
    public string ExpectedOutput { get; set; } = string.Empty;
    public string Remark { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public Guid CreatedBy { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public Guid UpdatedBy { get; set; }
    public List<string> Habitats { get; set; } = new List<string>();
    public List<string> ManipulationActivities { get; set; } = new List<string>();
    public List<string> NaturalCauses { get; set; } = new List<string>();
    public List<string> ManMadeCauses { get; set; } = new List<string>();
    public List<string> ManipulationDriver { get; set; } = new List<string>();
    public string? Image { get; set; } = string.Empty;
}

public record GetHabitatManipulationDto : HabitatManipulationDto
{
    public List<EcologySelectionDto> Habitats { get; set; } = new List<EcologySelectionDto>();
    public List<EcologySelectionDto> ManipulationActivities { get; set; } = new List<EcologySelectionDto>();
    public List<EcologySelectionDto> NaturalCauses { get; set; } = new List<EcologySelectionDto>();
    public List<EcologySelectionDto> ManMadeCauses { get; set; } = new List<EcologySelectionDto>();
    public List<EcologySelectionDto> ManipulationDriver { get; set; } = new List<EcologySelectionDto>();
}