using System.ComponentModel.DataAnnotations;
using conservation_backend.Features.Species;

namespace conservation_backend.Shared.Entities;

public class SpeciesOccurrence : BaseEntity
{
    public Guid Id { get; set; }

    public Guid SpeciesId { get; set; }
    public Species Species { get; set; } = null!;

    public Guid EntityId { get; set; }

    [MaxLength(50)]
    public string EntityName { get; set; } = string.Empty;
    
    public float? ObservedNumber { get; set; }
    
    public float? EstimatedNumber { get; set; }
    
    [MaxLength(150)]
    public string? StandardError { get; set; } = string.Empty;
}