using conservation_backend.Features.Files;
using conservation_backend.Features.Locations;
using conservation_backend.Features.Parks;
using conservation_backend.Features.Users;
using conservation_backend.Shared;
using conservation_backend.Shared.Dtos;

namespace conservation_backend.Features.FireBreaks;

public record FireBreakPaginationDto : PaginationDto
{
    public string? ParkId { get; set; }
}

public record FireBreakDto
{
    public Guid Id { get; set; }
    public Guid LocalAreaNameId { get; set; }
    public Guid ParkId { get; set; }
    public float Length { get; set; }
    public float Width { get; set; }
    public string Coordinates { get; set; } = string.Empty;
    public string Remark { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public string CreatedBy { get; set; } = string.Empty;
    public DateTime? UpdatedAt { get; set; }
    public string UpdatedBy { get; set; } = string.Empty;
    //Relationship
    public ParkDto Park { get; set; } = new ParkDto();
    public List<FileDto> File { get; set; } = new List<FileDto>();
    public LocationDto Location { get; set; } = new LocationDto();
    public UserMinimalDto Creator { get; set; } = new UserMinimalDto();
}

public record FireBreakRequestDto
{
    public Guid LocalAreaNameId { get; set; }
    public float Length { get; set; }
    public float Width { get; set; }
    public string Remark { get; set; } = string.Empty;
    public Guid CreatedBy { get; set; }
    public Guid? UpdatedBy { get; set; }
    public List<string> ClearanceMethods { get; set; } = new List<string>();
    public string? ImageId { get; set; } = string.Empty;
}

public record GetFireBreakDto : FireBreakDto
{
    public List<EcologySelectionDto> ClearanceMethods { get; set; } = new List<EcologySelectionDto>();
    public UserMinimalDto CreatedByUser { get; set; } = new UserMinimalDto();
    public UserMinimalDto UpdatedByUser { get; set; } = new UserMinimalDto();
}

public record FireBreakResponseDto : FireBreakDto
{
    public int RowNumber { get; set; }
}