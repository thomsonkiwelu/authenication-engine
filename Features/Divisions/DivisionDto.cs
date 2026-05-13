using conservation_backend.Features.Districts;
using conservation_backend.Features.Regions;
using conservation_backend.Features.Users;
using conservation_backend.Shared;

namespace conservation_backend.Features.Divisions;

public record DivisionPaginationDto : PaginationDto
{
    public string? Name { get; set; } = string.Empty;
    
    public string? RegionId { get; set; } = string.Empty;
    
    public string? DistrictId { get; set; } = string.Empty;
}

public record DivisionDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public Guid RegionId { get; set; }
    public Guid DistrictId { get; set; }
    public DateTime CreatedAt { get; set; }
    public Guid? CreatedBy { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public Guid? UpdatedBy { get; set; }
    //Relationship
    public RegionDto Region { get; set; } = new RegionDto();
    public DistrictDto District { get; set; } = new DistrictDto();
    public UserMinimalDto Creator { get; set; } = new UserMinimalDto();
}

public record DivisionRequest
{
    public Guid DistrictId { get; set; }
    public string Name { get; set; } = string.Empty;
}

public record DivisionResponseDto : DivisionDto
{
    public int RowNumber { get; set; }
}