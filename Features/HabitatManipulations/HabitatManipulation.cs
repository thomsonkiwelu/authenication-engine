using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using conservation_backend.Features.Locations;
using conservation_backend.Features.Parks;
using conservation_backend.Shared;

namespace conservation_backend.Features.HabitatManipulations;

public class HabitatManipulation: BaseEntity
{
    public Guid Id { get; set; }
    
    public Guid LocalAreaNameId { get; set; }
    [ForeignKey("LocalAreaNameId")]
    public Location Location { get; set; } = null!;
    
    public Guid ParkId { get; set; }
    public Park Park { get; set; } = null!;
    
    public Guid SpeciesId { get; set; }
    public Species.Species Species { get; set; } = null!;
    
    [MaxLength(50)]
    public string Session { get; set; } = string.Empty;
    
    [MaxLength(50)]
    public string Environment { get; set; } = string.Empty;
   
    public float AreaCovered { get; set; }
    
    [MaxLength(50)]
    public string ActionTaken { get; set; } = string.Empty;
    
    public float TotalNumber { get; set; }
    
    [MaxLength(50)]
    public string? SourceOfSeedling { get; set; } = string.Empty;
    
    [MaxLength(250)]
    public string? Coordinates { get; set; } = string.Empty;
    
    public string ExpectedOutput { get; set; } = string.Empty;
    
    public string Remark { get; set; } = string.Empty;
}