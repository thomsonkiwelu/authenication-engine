using System.ComponentModel.DataAnnotations;
using conservation_backend.Features.Species;

namespace conservation_backend.Shared.Entities;

public class MigratoryBird: BaseEntity
{
    public Guid Id { get; set; }
    
    public Guid SpeciesId { get; set; }
    public Species Species { get; set; } = null!;
    
    [MaxLength(50)]
    public string MigrationType { get; set; } = string.Empty;
    
    [MaxLength(50)]
    public string Arrival { get; set; } = string.Empty;
    
    [MaxLength(50)]
    public string Activity { get; set; } = string.Empty;
    
    public int IndividualObserved { get; set; }
    
    [MaxLength(250)]
    public string Remark { get; set; } = string.Empty;
    
    [MaxLength(50)]
    public string Category { get; set; } = string.Empty;
    
    public Guid EntityId { get; set; }
    
    [MaxLength(50)]
    public string EntityName { get; set; } = string.Empty;
}