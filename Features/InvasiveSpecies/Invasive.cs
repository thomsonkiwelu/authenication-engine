using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using conservation_backend.Features.Locations;
using conservation_backend.Features.Parks;
using conservation_backend.Shared;

namespace conservation_backend.Features.InvasiveSpecies;

public class Invasive : BaseEntity
{
    public Guid Id { get; set; }
        
    public Guid LocalAreaNameId { get; set; }
        
    [ForeignKey("LocalAreaNameId")]
    public Location Location { get; set; } = null!;
    
    public Guid SpeciesId { get; set; }
    public Species.Species Species { get; set; } = null!;
    
    [MaxLength(100)]
    public string ActivityType { get; set; } = string.Empty;
    
    [MaxLength(100)]
    public string? ControlType { get; set; } = string.Empty;
    
    [MaxLength(150)]
    public string? InfestedArea { get; set; } = string.Empty;
    
    [MaxLength(150)]
    public string? AreaCoverage { get; set; } = string.Empty;
    
    [MaxLength(150)]
    public string? BiologicalMethod { get; set; } = string.Empty;
    
    [MaxLength(150)]
    public string? BiologicalAgent { get; set; } = string.Empty;
    
    [MaxLength(150)]
    public string? OtherPossibleSource { get; set; } = string.Empty;
    
    [MaxLength(250)]
    public string? Coordinates { get; set; } = string.Empty;
    
    public string? Remark { get; set; } = string.Empty;
    
    public Guid? ParkId { get; set; }
    public Park? Park { get; set; } = null;
}
