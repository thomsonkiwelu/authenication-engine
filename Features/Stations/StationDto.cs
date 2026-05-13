using conservation_backend.Features.Parks;
using conservation_backend.Features.Users;
using conservation_backend.Shared;

namespace conservation_backend.Features.Stations;

public record StationPaginationDto : PaginationDto
{
    public string? ParkId { get; init; }
    
    public int? Category { get; init; }
}

public record StationRequest(
    string Name,
    string ParkId,
    string Type,
    int Category
);

public record StationDto
{
    public Guid Id { get; set; }
    
    public string Name { get; set; } = string.Empty;
    
    public string Type { get; set; } = string.Empty;
    
    public int Category { get; set; }
    
    public Guid ParkId { get; set; }
    
    public ParkDto Park { get; set; } = new ParkDto();
    
    public DateTime CreatedAt { get; set; }
    
    public string CreatedBy { get; set; } = string.Empty;
    
    public DateTime? UpdatedAt { get; set; }
    
    public string UpdatedBy { get; set; } = string.Empty;
}

public record StationResponseDto : StationDto
{
    public int RowNumber { get; set; }
    
    public UserMinimalDto Creator { get; set; } = new UserMinimalDto();
}