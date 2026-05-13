using conservation_backend.Features.LineTransects;
using conservation_backend.Features.Locations;
using conservation_backend.Features.Parks;
using conservation_backend.Features.Users;
using conservation_backend.Shared;
using conservation_backend.Shared.Dtos;

namespace conservation_backend.Features.BirdSurveys;

public record BirdSurveyPaginationDto: PaginationDto
{
    public string? ParkId { get; set; } = string.Empty;
}

public record BirdSurveyDto
{
    public Guid Id { get; set; }
    public Guid ParkId { get; set; }
    public Guid LocalAreaNameId { get; set; }
    public string Session { get; set; } = string.Empty;
    public string Habitat { get; set; } = string.Empty;
    public string Method { get; set; } = string.Empty;
    public int TotalIndividual { get; set; }
    public string? Coordinates { get; set; } = string.Empty;
    public string CreatedBy { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public string UpdatedBy { get; set; } = string.Empty;
    public DateTime? UpdatedAt { get; set; }
    //Relationship
    public ParkDto Park { get; set; } = new ParkDto();
    public LocationDto Location { get; set; } = new LocationDto();
    public UserMinimalDto CreatedByUser { get; set; } = new UserMinimalDto();
    public UserMinimalDto UpdatedByUser { get; set; } = new UserMinimalDto();
}

public record BirdSurveyRequestDto
{
    public Guid LocalAreaNameId { get; set; }
    public string Session { get; set; } = string.Empty;
    public string Habitat { get; set; } = string.Empty;
    public string Method { get; set; } = string.Empty;
    public string? Coordinates { get; set; } = string.Empty;
    public List<SpeciesObservedDto> SpeciesObserved { get; set; } = new List<SpeciesObservedDto>();
    public Guid? CreatedBy { get; set; }
    public Guid? UpdatedBy { get; set; }
}

public record BirdSurveyResponseDto : BirdSurveyDto
{
    public int RowNumber { get; set; }
}

public record BirdSuverySqlResponseDto
{
    public List<BirdSurveyResponseDto> Data { get; set; } = new();
    public PaginationMeta Meta { get; init; } = new ();
}

public record GetBirdSurveyDto : BirdSurveyDto
{
    public List<MigratoryBirdDto> MigratoryBirds { get; set; } = new List<MigratoryBirdDto>();
}