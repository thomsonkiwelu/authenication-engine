using System.ComponentModel.DataAnnotations;
using conservation_backend.Features.Parks;
using conservation_backend.Shared;

namespace conservation_backend.Features.AerialCensuses;

public class AerialCensus: BaseEntity
{
    public Guid Id { get; set; }
    
    public Guid ParkId { get; set; }
    public Park Park { get; set; } = null!;
    
    [MaxLength(50)]
    public string AreaInvolved { get; set; } = string.Empty;
    
    public float AreaCovered { get; set; }
    
    [MaxLength(50)]
    public string CensusType { get; set; } = string.Empty;
    
    [MaxLength(50)]
    public string Session { get; set; } = string.Empty;
    
    [MaxLength(255)]
    public string? Coordinates { get; set; } = string.Empty;
    
    public DateTime StartDate { get; set; }
    
    public DateTime EndDate { get; set; }
}