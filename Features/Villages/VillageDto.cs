using conservation_backend.Features.Districts;
using conservation_backend.Features.Divisions;
using conservation_backend.Features.Regions;
using conservation_backend.Features.Users;
using conservation_backend.Features.Wards;
using conservation_backend.Shared;

namespace conservation_backend.Features.Villages;

public record VillagePaginationDto : PaginationDto
{
    public string? Name { get; set; } = string.Empty;
    public string? RegionId { get; set; } = string.Empty;
    public string? DistrictId { get; set; } = string.Empty;
    public string? DivisionId { get; set; } = string.Empty;
    public string? WardId { get; set; } = string.Empty;
}

public record VillageDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public Guid RegionId { get; set; }
    public Guid DistrictId { get; set; }
    public Guid? DivisionId { get; set; }
    public Guid WardId { get; set; }
    public DateTime CreatedAt { get; set; }
    public Guid? CreatedBy { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public Guid? UpdatedBy { get; set; }
    //Relationship
    public RegionDto Region { get; set; } = new RegionDto();
    public DistrictDto District { get; set; } = new DistrictDto();
    public DivisionDto Division { get; set; } = new DivisionDto();
    public WardDto Ward { get; set; } = new WardDto();
    public UserMinimalDto Creator { get; set; } = new UserMinimalDto();
}

public record VillageRequest
{
    public Guid WardId { get; set; }
    public Guid? DivisionId { get; set; }
    public string Name { get; set; } = string.Empty;
}

public record VillageResponseDto : VillageDto
{
    public int RowNumber { get; set; }
}