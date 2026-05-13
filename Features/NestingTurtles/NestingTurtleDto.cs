using conservation_backend.Features.Files;
using conservation_backend.Features.Locations;
using conservation_backend.Features.Parks;
using conservation_backend.Features.Users;
using conservation_backend.Shared;

namespace conservation_backend.Features.NestingTurtles;

public record NestingTurtlePaginationDto : PaginationDto
{
    public string? ParkId { get; init; }
}

public record NestingTurtleDto
{
    public Guid Id { get; set; }
    public Guid LocalAreaNameId { get; set; }
    public Guid ParkId { get; set; }
    public float HatchedEggs { get; set; }
    public float UnHatchedEggs { get; set; }
    public float Hatchling { get; set; }
    public float PoachedEggs { get; set; }
    public string? Coordinates { get; set; } = string.Empty;
    public string Remark { get; set; } = string.Empty;
    public string CreatedBy { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public string UpdatedBy { get; set; } = string.Empty;
    public DateTime? UpdatedAt { get; set; }
    //Relationship
    public ParkDto Park { get; set; } = new ParkDto();
    public FileDto? File { get; set; } = new FileDto();
    public LocationDto Location { get; set; } = new LocationDto();
    public UserMinimalDto Creator { get; set; } = new UserMinimalDto();
    public UserMinimalDto Updater { get; set; } = new UserMinimalDto();
}

public record NestingTurtleRequestDto
{
    public Guid LocalAreaNameId { get; set; }
    public Guid ParkId { get; set; }
    public float HatchedEggs { get; set; }
    public float UnHatchedEggs { get; set; }
    public float Hatchling { get; set; }
    public float PoachedEggs { get; set; }
    public string? Coordinates { get; set; } = string.Empty;
    public string Remark { get; set; } = string.Empty;
    public string? ImageId { get; set; } = string.Empty;
    public Guid? CreatedBy { get; set; }
    public Guid? UpdatedBy { get; set; }
}

public record NestingTurtleResponseDto : NestingTurtleDto
{
    public int RowNumber { get; set; }
}