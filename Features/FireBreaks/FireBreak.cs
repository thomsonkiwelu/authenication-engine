using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using conservation_backend.Features.Locations;
using conservation_backend.Features.Parks;
using conservation_backend.Shared;

namespace conservation_backend.Features.FireBreaks;

public class FireBreak: BaseEntity
{
    public Guid Id { get; set; }
    
    public Guid LocalAreaNameId { get; set; }
    [ForeignKey("LocalAreaNameId")]
    public Location Location { get; set; } = null!;
    
    public Guid ParkId { get; set; }
    public Park Park { get; set; } = null!;
    
    public float Length { get; set; }
    
    public float Width { get; set; }
    
    [MaxLength(250)]
    public string? Coordinates { get; set; } = string.Empty;
    
    public string Remark { get; set; } = string.Empty;
}