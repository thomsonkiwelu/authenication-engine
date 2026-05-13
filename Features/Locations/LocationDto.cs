using conservation_backend.Features.Parks;
using conservation_backend.Features.Users;
using conservation_backend.Shared;

namespace conservation_backend.Features.Locations;

public record LocationPaginationDto : PaginationDto
{
    public string? ParkId { get; init; }
}

public record LocationRequest(
    string Name,
    string ParkId
);

public record LocationDto
{
    public Guid Id { get; set; }

    public string Name { get; set; } = string.Empty;
    
    public Guid ParkId { get; set; }
    
    public ParkDto Park { get; set; } = new ParkDto();
    
    public DateTime CreatedAt { get; set; }
    
    public string CreatedBy { get; set; } = string.Empty;
}

public record LocationResponseDto : LocationDto
{
    public int RowNumber { get; set; }
    
    public UserMinimalDto Creator { get; set; } = new UserMinimalDto();
}