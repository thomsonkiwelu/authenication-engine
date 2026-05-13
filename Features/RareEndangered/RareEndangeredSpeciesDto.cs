using conservation_backend.Features.Files;
using conservation_backend.Features.Locations;
using conservation_backend.Features.Parks;
using conservation_backend.Features.Species;
using conservation_backend.Features.Users;
using conservation_backend.Shared;
using conservation_backend.Shared.Dtos;

namespace conservation_backend.Features.RareEndangered;

public record RareEndangeredSpeciesPaginationDto: PaginationDto
{
    public string? ParkId { get; set; } = string.Empty;
    
    public string? Category { get; set; } = string.Empty;
}

public record RareEndangeredSpeciesDto
{
    public Guid Id { get; set; }
    public Guid ParkId { get; set; }
    public string Session { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public string NameKeyInformer { get; set; } = string.Empty;
    public string KeySourceOfInformation { get; set; } = string.Empty;
    public int TotalIndividual { get; set; }
    public string CreatedBy { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public string UpdatedBy { get; set; } = string.Empty;
    public DateTime? UpdatedAt { get; set; }
    //Relationship
    public ParkDto Park { get; set; } = new ParkDto();
    public UserMinimalDto CreatedByUser { get; set; } = new UserMinimalDto();
    public UserMinimalDto UpdatedByUser { get; set; } = new UserMinimalDto();
}

public record RareSpeciesOccurrenceDto
{
    public Guid Id { get; set; }
    public Guid LocalAreaNameId { get; set; }
    public Guid SpeciesId { get; set; }
    public Guid RareEndangeredSpeciesId { get; set; }
    public string VegetationCategory { get; set; } = string.Empty;
    public float NumberOfIndividual { get; set; }
    public string Remark { get; set; } = string.Empty;
    public string? Coordinates { get; set; } = string.Empty;
    public string CreatedBy { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public string UpdatedBy { get; set; } = string.Empty;
    public DateTime? UpdatedAt { get; set; }
    //Relationship
    public SpeciesDto Species { get; set; } = new SpeciesDto();
    public LocationDto Location { get; set; } = new LocationDto();
    public FileDto? File { get; set; } = new FileDto();
}

public record RareEndangeredSpeciesRequestDto
{
    public Guid ParkId { get; set; }
    public string Session { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public string NameKeyInformer { get; set; } = string.Empty;
    public List<string> SourceOfInformation { get; set; } = new List<string>();
    public List<RareSpeciesOccurrenceRequest> AnimalObserved { get; set; } = new List<RareSpeciesOccurrenceRequest>();
    public Guid? CreatedBy { get; set; }
    public Guid? UpdatedBy { get; set; }
}

public record RareSpeciesOccurrenceRequest
{
    public Guid LocalAreaNameId { get; set; }
    public Guid SpeciesId { get; set; }
    public string VegetationCategory { get; set; } = string.Empty;
    public float NumberOfIndividual { get; set; }
    public string Remark { get; set; } = string.Empty;
    public string? Coordinates { get; set; } = string.Empty;
    public string? ImageId { get; set; } = string.Empty;
}

public record RareEndangeredSpeciesResponseDto : RareEndangeredSpeciesDto
{
    public int RowNumber { get; set; }
}

public record RareEndangeredSpeciesSqlResponseDto
{
    public List<RareEndangeredSpeciesResponseDto> Data { get; set; } = new();
    public PaginationMeta Meta { get; init; } = new ();
}

public record GetRareEndangeredSpeciesDto : RareEndangeredSpeciesDto
{
    public List<RareSpeciesOccurrenceDto> RareSpeciesOccurrences { get; set; } = new List<RareSpeciesOccurrenceDto>();
    public List<EcologySelectionDto> SourceOfInformation { get; set; } = new List<EcologySelectionDto>();
}