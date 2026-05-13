using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using conservation_backend.Features.Locations;
using conservation_backend.Features.Parks;
using conservation_backend.Shared;

namespace conservation_backend.Features.RareEndangered;

public class RareEndangeredSpecies: BaseEntity
{
    public Guid Id { get; set; }
    
    public Guid ParkId { get; set; }
    public Park Park { get; set; } = null!;
    
    [MaxLength(50)]
    public string Session { get; set; } = string.Empty;
    
    [MaxLength(50)]
    public string Category { get; set; } = string.Empty;
    
    [MaxLength(100)]
    public string? NameKeyInformer { get; set; } = string.Empty;
}

public class RareSpeciesOccurrence : BaseEntity
{
    public Guid Id { get; set; }
    
    public Guid LocalAreaNameId { get; set; }
    [ForeignKey("LocalAreaNameId")]
    public Location Location { get; set; } = null!;
    
    public Guid SpeciesId { get; set; }
    public Species.Species Species { get; set; } = null!;
    
    public Guid RareEndangeredSpeciesId { get; set; }
    [ForeignKey("RareEndangeredSpeciesId")]
    public RareEndangeredSpecies RareEndangeredSpecies { get; set; } = null!;
    
    [MaxLength(100)]
    public string VegetationCategory { get; set; } = string.Empty;
    
    public float NumberOfIndividual { get; set; }
    
    [MaxLength(255)]
    public string Remark { get; set; } = string.Empty;
    
    [MaxLength(250)]
    public string? Coordinates { get; set; } = string.Empty;
}