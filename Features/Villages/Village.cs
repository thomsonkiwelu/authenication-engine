using System.ComponentModel.DataAnnotations;
using conservation_backend.Features.Districts;
using conservation_backend.Features.Regions;
using conservation_backend.Features.Wards;
using conservation_backend.Shared;

namespace conservation_backend.Features.Villages;

public class Village: BaseEntity
{
    public Guid Id { get; set; }
    
    [MaxLength(50)]
    public string Name { get; set; } =  string.Empty;
    
    public Guid RegionId { get; set; }
    public Region Region { get; set; } = null!;
    
    public Guid DistrictId { get; set; }
    public District District { get; set; } = null!;
    
    public Guid? DivisionId { get; set; }
    public District? Division { get; set; } = null;
    
    public Guid WardId { get; set; }
    public Ward Ward { get; set; } = null!;
}