using System.ComponentModel.DataAnnotations;
using conservation_backend.Features.Districts;
using conservation_backend.Features.Regions;
using conservation_backend.Shared;

namespace conservation_backend.Features.Divisions;

public class Division: BaseEntity
{
    public Guid Id { get; set; }
    
    [MaxLength(50)]
    public string Name { get; set; } =  string.Empty;
    
    public Guid RegionId { get; set; }
    public Region Region { get; set; } = null!;
    
    public Guid DistrictId { get; set; }
    public District District { get; set; } = null!;
}