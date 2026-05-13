using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using conservation_backend.Features.Locations;
using conservation_backend.Features.Parks;
using conservation_backend.Shared;

namespace conservation_backend.Features.WildFires;

public class WildFire: BaseEntity
{
    public Guid Id { get; set; }
    
    public Guid LocalAreaNameId { get; set; }
    [ForeignKey("LocalAreaNameId")]
    public Location Location { get; set; } = null!;
    
    public Guid ParkId { get; set; }
    public Park Park { get; set; } = null!;
    
    [MaxLength(50)]
    public string? SourceOfFire { get; set; } = string.Empty;
    
    public float BurntArea { get; set; }
    
    public float BurningDuration { get; set; }
    
    public float ParticipantStaff { get; set; }
    
    public float OtherParticipant { get; set; }
    
    [MaxLength(50)]
    public string? OtherFireSource { get; set; } = string.Empty;
    
    [MaxLength(250)]
    public string? Coordinates { get; set; } = string.Empty;
    
    public string Remark { get; set; } = string.Empty;
}