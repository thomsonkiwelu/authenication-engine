using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using conservation_backend.Features.Locations;
using conservation_backend.Features.Parks;
using conservation_backend.Shared;

namespace conservation_backend.Features.BirdSurveys;

public class BirdSurvey: BaseEntity
{
    public Guid Id { get; set; }
    
    public Guid LocalAreaNameId { get; set; }
    [ForeignKey("LocalAreaNameId")]
    public Location Location { get; set; } = null!;
    
    public Guid ParkId { get; set; }
    public Park Park { get; set; } = null!; 
    
    [MaxLength(50)]
    public string Method { get; set; } = string.Empty;
    
    [MaxLength(50)]
    public string Habitat { get; set; } = string.Empty;
    
    [MaxLength(50)]
    public string Session { get; set; } = string.Empty;
    
    [MaxLength(250)]
    public string? Coordinates { get; set; } = string.Empty;
}