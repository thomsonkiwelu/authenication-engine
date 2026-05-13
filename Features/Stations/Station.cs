using System.ComponentModel.DataAnnotations;
using authentication_engine.Features.Parks;
using authentication_engine.Shared.Entities;

namespace authentication_engine.Features.Stations;

public class Station: BaseEntity
{
    public Guid Id { get; set; }

    [MaxLength(150)]
    public string Name { get; set; } = string.Empty;
    
    [MaxLength(150)]
    public string Type { get; set; } = string.Empty;
    
    public int Category { get; set; } //1=weather 2=waste
    
    public Guid ParkId { get; set; }
    
    public Park Park { get; set; } = null!;
}