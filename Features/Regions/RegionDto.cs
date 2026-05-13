using conservation_backend.Features.Users;
using conservation_backend.Shared;

namespace conservation_backend.Features.Regions;

public record RegionPaginationDto : PaginationDto
{
    public string? Name { get; set; } = string.Empty;
}

public record RegionDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public Guid? CreatedBy { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public Guid? UpdatedBy { get; set; }
    //Relationship
    public UserMinimalDto Creator { get; set; } = new UserMinimalDto();
}

public record RegionRequest
{
    public string Name { get; set; } = string.Empty;
}

public record RegionResponseDto : RegionDto
{
    public int RowNumber { get; set; }
}