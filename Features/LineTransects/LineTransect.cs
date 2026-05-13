using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using conservation_backend.Features.Locations;
using conservation_backend.Features.Parks;
using conservation_backend.Shared;

namespace conservation_backend.Features.LineTransects;

public class LineTransect: BaseEntity
{
    public Guid Id { get; set; }
    
    public Guid ParkId { get; set; }
    public Park Park { get; set; } = null!;
    
    [MaxLength(50)]
    public string Session { get; set; } = string.Empty;
    
    [MaxLength(250)]
    public string? LineTransectStartCoordinates { get; set; } = string.Empty;
    
    [MaxLength(250)]
    public string? LineTransectRecordAltitude { get; set; } = string.Empty;
    
    [MaxLength(50)]
    public string LineTransectHabitat { get; set; } = string.Empty;
    
    public Guid LineTransectLocalAreaNameId { get; set; }
    [ForeignKey("LineTransectLocalAreaNameId")]
    public Location LineTransectLocation { get; set; } = null!;
    
    [MaxLength(250)]
    public string? AlongTransectCoordinates { get; set; } = string.Empty;
    
    [MaxLength(250)]
    public string? AlongTransectRecordAltitude { get; set; } = string.Empty;
    
    [MaxLength(250)]
    public string AlongTransectHabitat { get; set; } = string.Empty;
    
    public Guid AlongTransectLocalAreaNameId { get; set; }
    [ForeignKey("AlongTransectLocalAreaNameId")]
    public Location AlongTransectLocation { get; set; } = null!;
    
    [MaxLength(250)]
    public string? EndTransectCoordinates { get; set; } = string.Empty;
    
    [MaxLength(250)]
    public string? EndTransectRecordAltitude { get; set; } = string.Empty;
}
