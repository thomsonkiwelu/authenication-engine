using conservation_backend.Features.Locations;
using conservation_backend.Features.Parks;
using conservation_backend.Features.Users;
using conservation_backend.Shared;
using conservation_backend.Shared.Dtos;

namespace conservation_backend.Features.LineTransects;

public record LineTransectPaginationDto: PaginationDto
{
    public string? ParkId { get; set; } = string.Empty;
}

public record LineTransectDto
{
    public Guid Id { get; set; }
    public Guid ParkId { get; set; }
    public string Session { get; set; } = string.Empty;
    public string? LineTransectStartCoordinates { get; set; } = string.Empty;
    public string? LineTransectRecordAltitude { get; set; } = string.Empty;
    public string? LineTransectHabitat { get; set; } = string.Empty;
    public Guid LineTransectLocalAreaNameId { get; set; }
    public string? AlongTransectCoordinates { get; set; } = string.Empty;
    public string? AlongTransectRecordAltitude { get; set; } = string.Empty;
    public string? AlongTransectHabitat { get; set; } = string.Empty;
    public Guid AlongTransectLocalAreaNameId { get; set; }
    public string? EndTransectCoordinates { get; set; } = string.Empty;
    public string? EndTransectRecordAltitude { get; set; } = string.Empty;
    public int TotalMigratoryBirdCount { get; set; }
    public string CreatedBy { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public string UpdatedBy { get; set; } = string.Empty;
    public DateTime? UpdatedAt { get; set; }
    //Relationship
    public ParkDto Park { get; set; } = new ParkDto();
    public LocationDto AlongTransectLocation { get; set; } = new LocationDto();
    public LocationDto LineTransectLocation { get; set; } = new LocationDto();
    public UserMinimalDto CreatedByUser { get; set; } = new UserMinimalDto();
}

public record LineTransectRequestDto
{
    public Guid ParkId { get; set; }
    public string Session { get; set; } = string.Empty;
    public string? LineTransectStartCoordinates { get; set; } = string.Empty;
    public string? LineTransectRecordAltitude { get; set; } = string.Empty;
    public string? LineTransectHabitat { get; set; } = string.Empty;
    public Guid LineTransectLocalAreaNameId { get; set; }
    public string? AlongTransectCoordinates { get; set; } = string.Empty;
    public string? AlongTransectRecordAltitude { get; set; } = string.Empty;
    public string? AlongTransectHabitat { get; set; } = string.Empty;
    public Guid AlongTransectLocalAreaNameId { get; set; }
    public string? EndTransectCoordinates { get; set; } = string.Empty;
    public string? EndTransectRecordAltitude { get; set; } = string.Empty;
    public List<SpeciesObservedDto> LineTransectSpeciesObserved { get; set; } = new List<SpeciesObservedDto>();
    public List<SpeciesObservedDto> AlongTransectSpeciesObserved { get; set; } = new List<SpeciesObservedDto>();
    public Guid? CreatedBy { get; set; }
    public Guid? UpdatedBy { get; set; }
}

public record SpeciesObservedDto
{
    public Guid SpeciesId { get; set; }
    public string MigrationType { get; set; } = string.Empty;
    public string Arrival { get; set; } = string.Empty;
    public string Activity { get; set; } = string.Empty;
    public int IndividualObserved { get; set; }
    public string Remark { get; set; } = string.Empty;
    public string? ImageId { get; set; } = string.Empty;
    public string? ImageUrl { get; set; } = string.Empty;
}

public record LineTransectResponseDto : LineTransectDto
{
    public int RowNumber { get; set; }
}

public record LineTransectSqlResponseDto
{
    public List<LineTransectResponseDto> Data { get; set; } = new();
    public PaginationMeta Meta { get; init; } = new ();
}

public record GetLineTransectDto : LineTransectDto
{
    public List<MigratoryBirdDto> LineTransectMigratoryBirds { get; set; } = new List<MigratoryBirdDto>();
    public List<MigratoryBirdDto> AlongTransectMigratoryBirds { get; set; } = new List<MigratoryBirdDto>();
    public UserMinimalDto UpdatedByUser { get; set; } = new UserMinimalDto();
}