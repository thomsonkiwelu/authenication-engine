using System.ComponentModel.DataAnnotations;
using conservation_backend.Features.Regions;
using conservation_backend.Shared;

namespace conservation_backend.Features.Districts;

public class District : BaseEntity
{
    public Guid Id { get; set; }
    
    [MaxLength(50)]
    public string Name { get; set; } =  string.Empty;
    
    public Guid RegionId { get; set; }
    public Region Region { get; set; } = null!;
}