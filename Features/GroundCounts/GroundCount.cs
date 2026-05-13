using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using conservation_backend.Features.Locations;
using conservation_backend.Features.Parks;
using conservation_backend.Shared;

namespace conservation_backend.Features.GroundCounts;

public class GroundCount: BaseEntity
{
    public Guid Id { get; set; }
    
    public Guid ParkId { get; set; }
    public Park Park { get; set; } = null!;
    
    public Guid LocalAreaNameId { get; set; }
    [ForeignKey("LocalAreaNameId")]
    public Location Location { get; set; } = null!;
    
    [MaxLength(250)]
    public string TransectId { get; set; } = string.Empty;
    
    [MaxLength(50)]
    public string Method { get; set; } = string.Empty;
    
    [MaxLength(250)]
    public string TransectStartingPoint { get; set; } = string.Empty;
    
    [MaxLength(250)]
    public string TransectEndPoint { get; set; } = string.Empty;
    
    public float EndDistance { get; set; }
    
    [MaxLength(250)]
    public string RouteDescription { get; set; } = string.Empty;
    
    [MaxLength(50)]
    public string WeatherCondition { get; set; } = string.Empty;
    
    [MaxLength(50)]
    public string? OtherWeatherCondition { get; set; } = string.Empty;
}

public class GroundCountSighting : BaseEntity
{
    public Guid Id { get; set; }
    
    public Guid SpeciesId { get; set; }
    public Species.Species Species { get; set; } = null!;
    
    public Guid GroundCountId { get; set; }
    [ForeignKey("GroundCountId")]
    public GroundCount GroundCount { get; set; } = null!;
    
    [MaxLength(50)]
    public string TimeOfSighting { get; set; } = string.Empty;
    
    public float PerpendicularDistance { get; set; }
    
    public float Distance { get; set; }
    
    [MaxLength(250)]
    public string? Coordinates { get; set; } = string.Empty;
    
    [MaxLength(250)]
    public string Remark { get; set; } = string.Empty;
}
