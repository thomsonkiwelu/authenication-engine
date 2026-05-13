using conservation_backend.Features.Files;
using conservation_backend.Features.Locations;
using conservation_backend.Features.Parks;
using conservation_backend.Features.Species;
using conservation_backend.Features.Users;
using conservation_backend.Shared;
using conservation_backend.Shared.Dtos;

namespace conservation_backend.Features.InvasiveSpecies;

public record InvasivePaginationDto: PaginationDto
{
    public string? ActivityType { get; set; } = string.Empty;
    
    public string? ParkId { get; set; } = string.Empty;
}

public record InvasiveDto
{
    public Guid Id { get; set; }
    public Guid LocalAreaNameId { get; set; }
    public Guid SpeciesId { get; set; }
    public Guid ParkId { get; set; }
    public string ActivityType { get; set; } = string.Empty;
    public string ControlType { get; set; } = string.Empty;
    public string InfestedArea { get; set; } = string.Empty;
    public string AreaCoverage { get; set; } = string.Empty;
    public string BiologicalMethod { get; set; } = string.Empty;
    public string BiologicalAgent { get; set; } = string.Empty;
    public string OtherPossibleSource { get; set; } = string.Empty;
    public string Coordinates { get; set; } = string.Empty;
    public string Remark { get; set; } = string.Empty;
    public string CreatedBy { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public string UpdatedBy { get; set; } = string.Empty;
    public DateTime? UpdatedAt { get; set; }
    //Relationship
    public ParkDto Park { get; set; } = new ParkDto();
    public FileDto? File { get; set; } = new FileDto();
    public SpeciesDto Species { get; set; } = new SpeciesDto();
    public LocationDto Location { get; set; } = new LocationDto();
    public UserMinimalDto CreatedByUser { get; set; } = new UserMinimalDto();
    public UserMinimalDto UpdatedByUser { get; set; } = new UserMinimalDto();
}
    
public record InvasiveRequestDto
{
    public Guid LocalAreaNameId { get; set; }
    public Guid SpeciesId { get; set; }
    public string ActivityType { get; set; } = string.Empty;
    public string? ControlType { get; set; } = string.Empty;
    public string? InfestedArea { get; set; } = string.Empty;
    public string? AreaCoverage { get; set; } = string.Empty;
    public string? BiologicalMethod { get; set; } = string.Empty;
    public string? BiologicalAgent { get; set; } = string.Empty;
    public string? OtherPossibleSource { get; set; } = string.Empty;
    public string? Coordinates { get; set; } = string.Empty;
    public string? Remark { get; set; } = string.Empty;
    public List<string> PossibleSources { get; set; } = new List<string>();
    public List<string> ChemicalMethods { get; set; } = new List<string>();
    public List<string> MechanicalMethods { get; set; } = new List<string>();
    public List<OtherSpecieObservedDto> OtherSpecieObserved { get; set; } = new List<OtherSpecieObservedDto>();
    public string? ImageId { get; set; } = string.Empty;
    public Guid? CreatedBy { get; set; }
    public Guid? UpdatedBy { get; set; }
}

public record OtherSpecieObservedDto
{
    public string SpeciesId { get; set; } = string.Empty;
}

public record InvasiveResponseDto : InvasiveDto
{
    public int RowNumber { get; set; }
}

public record ListInvasiveSpeciesSqlResponseDto
{
    public List<InvasiveResponseDto> Data { get; set; } = new();
    public PaginationMeta Meta { get; init; } = new ();
}

public record GetInvasiveSpeciesDto : InvasiveDto
{
    public List<SpeciesOccurrenceDto> SpeciesOccurrences { get; set; } = new List<SpeciesOccurrenceDto>();
    public List<EcologySelectionDto> ChemicalMethods { get; set; } = new List<EcologySelectionDto>();
    public List<EcologySelectionDto> MechanicalMethods { get; set; } = new List<EcologySelectionDto>();
    public List<EcologySelectionDto> PossibleSources { get; set; } = new List<EcologySelectionDto>();
}
    
