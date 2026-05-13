using conservation_backend.Features.Parks;
using conservation_backend.Features.Users;
using conservation_backend.Shared;
using conservation_backend.Shared.Dtos;

namespace conservation_backend.Features.AerialCensuses;

public record AerialCensusPaginationDto : PaginationDto
{
    public string? StartDate { get; set; }
    public string? EndDate { get; set; }
    public string? ParkId { get; set; }
}
    
public record AerialCensusDto
{
    public Guid Id { get; set; }
    public Guid ParkId { get; set; }
    public string Session { get; set; } = string.Empty;
    public string AreaInvolved { get; set; } = string.Empty;
    public float AreaCovered { get; set; }
    public string CensusType { get; set; } = string.Empty;
    public string StartDate { get; set; } = string.Empty;
    public string EndDate { get; set; } = string.Empty;
    public string? Coordinates { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public string CreatedBy { get; set; } = string.Empty;
    public DateTime? UpdatedAt { get; set; }
    public string UpdatedBy { get; set; } = string.Empty;
    //Relationship
    public ParkDto Park { get; set; } = new ParkDto();
    public UserMinimalDto CreatedByUser { get; set; } = new UserMinimalDto();
    public UserMinimalDto UpdatedByUser { get; set; } = new UserMinimalDto();
}

public record AerialCensusResponseDto : AerialCensusDto
{
    public int RowNumber { get; set; }
    public int TotalObservedSpecies { get; set; }
    public int TotalEstimatedSpecies { get; set; }
}

public record AerialCensusSqlResponseDto
{
    public List<AerialCensusResponseDto> Data { get; set; } = new();
    public PaginationMeta Meta { get; init; } = new ();
}

public record AerialCensusRequestDto
{
    public Guid ParkId { get; set; }
    public string Session { get; set; } = string.Empty;
    public string AreaInvolved { get; set; } = string.Empty;
    public float AreaCovered { get; set; }
    public string CensusType { get; set; } = string.Empty;
    public string StartDate { get; set; } = string.Empty;
    public string EndDate { get; set; } = string.Empty;
    public string? Coordinates { get; set; } = string.Empty;
    public List<SpecieObservedDto> SpeciesObserved { get; set; } = new List<SpecieObservedDto>();
    public DateTime CreatedAt { get; set; }
    public Guid CreatedBy { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public Guid UpdatedBy { get; set; }
    public string? Image { get; set; } = string.Empty;
}

public record SpecieObservedDto
{
    public string SpeciesId { get; set; } = string.Empty;
    public float ObservedNumber { get; set; }
    public float EstimatedNumber { get; set; }
    public string StandardError { get; set; } = string.Empty;
}

public record GetAerialCensusDto : AerialCensusDto
{
    public List<SpeciesOccurrenceDto> SpeciesOccurrences { get; set; } = new List<SpeciesOccurrenceDto>();
}