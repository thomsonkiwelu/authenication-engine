using System.ComponentModel.DataAnnotations;
using conservation_backend.Features.Parks;
using conservation_backend.Shared;

namespace conservation_backend.Features.MangabeyMonitoring;

public class MangabeyMonitoring: BaseEntity
{
    public Guid Id { get; set; }
    
    public Guid ParkId { get; set; }
    public Park Park { get; set; } = null!;
    
    [MaxLength(50)]
    public string ActivityType { get; set; } = string.Empty;
    
    public int NumberOfParticipant { get; set; }
    
    public int? NumberOfSpecies { get; set; }
    
    [MaxLength(250)]
    public string? Coordinates { get; set; } = string.Empty;
}