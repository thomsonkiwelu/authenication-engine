using conservation_backend.Features.Files;
using conservation_backend.Features.Parks;
using conservation_backend.Features.Users;
using conservation_backend.Features.WaterBodies;
using conservation_backend.Shared;

namespace conservation_backend.Features.WaterQuantities;

public record WaterQuantityPaginationDto : PaginationDto
{
    public string? ParkId { get; set; }
    
    public string? WaterBodyId { get; set; }
}

public record WaterQuantityDto
{
    public Guid Id { get; set; }
    public Guid WaterBodyId { get; set; }
    public Guid ParkId { get; set; }
    public float WaterLevel { get; set; }
    public float WettedPerimeter { get; set; }
    public float WettedWidth { get; set; }
    public float AverageDepth { get; set; }
    public float Length { get; set; }
    public float AverageTime { get; set; }
    public float MinimumFlowRate { get; set; }
    public float MaximumFlowRate { get; set; }
    public float AverageFlowRate { get; set; }
    public float CalculatedDischargeRate { get; set; }
    public float Volume { get; set; }
    public string? Coordinate { get; set; } = string.Empty;
    public string Remark { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public string CreatedBy { get; set; } = string.Empty;
    public DateTime? UpdatedAt { get; set; }
    public string UpdatedBy { get; set; } = string.Empty;
    //Relationship
    public ParkDto Park { get; set; } = new ParkDto();
    public List<FileDto> Attachments { get; set; } = new List<FileDto>();
    public WaterBodyDto WaterBody { get; set; } = new WaterBodyDto();
    public UserMinimalDto Creator { get; set; } = new UserMinimalDto();
    public UserMinimalDto Updater { get; set; } = new UserMinimalDto();
}

public record  WaterQuantityResponseDto
{
    public Guid Id { get; set; }
    public int RowNumber { get; set; }
    public Guid WaterBodyId { get; set; }
    public Guid ParkId { get; set; }
    public float Volume { get; set; }
    public float WaterLevel { get; set; }
    public DateTime CreatedAt { get; set; }
    public string CreatedBy { get; set; } = string.Empty;
    public DateTime? UpdatedAt { get; set; }
    public string UpdatedBy { get; set; } = string.Empty;
    public ParkDto Park { get; set; } = new ParkDto();
    public WaterBodyDto WaterBody { get; set; } = new WaterBodyDto();
    public UserMinimalDto Creator { get; set; } = new UserMinimalDto();
}

public record WaterQuantityRequestDto
{
    public Guid WaterBodyId { get; set; }
    public float WaterLevel { get; set; }
    public float WettedPerimeter { get; set; }
    public float WettedWidth { get; set; }
    public float AverageDepth { get; set; }
    public float Length { get; set; }
    public float AverageTime { get; set; }
    public float MinimumFlowRate { get; set; }
    public float MaximumFlowRate { get; set; }
    public float AverageFlowRate { get; set; }
    public float CalculatedDischargeRate { get; set; }
    public float Volume { get; set; }
    public string? Coordinate { get; set; } = string.Empty;
    public string Remark { get; set; } = string.Empty;
    public string? ImageId { get; set; } = string.Empty;
    public string? FileAttachmentId { get; set; } = string.Empty;
}