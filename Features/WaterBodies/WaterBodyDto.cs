using conservation_backend.Features.Parks;
using conservation_backend.Features.Users;
using conservation_backend.Shared;

namespace conservation_backend.Features.WaterBodies;

public record WaterBodyPaginationDto : PaginationDto
{
    public string? ParkId { get; set; }
    public string? Type { get; set; }
}

public record WaterBodyRequest(
    string Name,
    string ParkId,
    string Type
);

public record WaterBodyDto
{
    public Guid Id { get; set; }
    
    public string Name { get; set; } = string.Empty;
    
    public string Type { get; set; } = string.Empty;
    
    public Guid ParkId { get; set; }
    
    public ParkDto Park { get; set; } = new ParkDto();
    
    public DateTime CreatedAt { get; set; }
    
    public string CreatedBy { get; set; } = string.Empty;
    
    public DateTime? UpdatedAt { get; set; }
    
    public string UpdatedBy { get; set; } = string.Empty;
}

public record  WaterBodyResponseDto :  WaterBodyDto
{
    public int RowNumber { get; set; }
    
    public UserMinimalDto CreatedByUser { get; set; } = new UserMinimalDto();
}