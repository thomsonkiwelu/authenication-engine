using conservation_backend.Features.Regions;
using conservation_backend.Features.Users;
using conservation_backend.Shared;

namespace conservation_backend.Features.Districts;

public record DistrictPaginationDto : PaginationDto
{
    public string? Name { get; set; } = string.Empty;
    
    public string? RegionId { get; set; } = string.Empty;
}

public record DistrictDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public Guid RegionId { get; set; }
    public DateTime CreatedAt { get; set; }
    public Guid? CreatedBy { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public Guid? UpdatedBy { get; set; }
    //Relationship
    public RegionDto Region { get; set; } = new RegionDto();
    public UserMinimalDto Creator { get; set; } = new UserMinimalDto();
}

public record DistrictRequest
{
    public Guid RegionId { get; set; }
    public string Name { get; set; } = string.Empty;
}

public record DistrictResponseDto : DistrictDto
{
    public int RowNumber { get; set; }
}