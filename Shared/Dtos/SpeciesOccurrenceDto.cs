using conservation_backend.Features.Species;

namespace conservation_backend.Shared.Dtos;

public class SpeciesOccurrenceDto
{
    public Guid Id { get; set; }
    
    public Guid SpeciesId { get; set; }
    
    public Guid EntityId { get; set; }
    
    public string EntityName { get; set; } = string.Empty;
    
    public float? ObservedNumber { get; set; }
    
    public float? EstimatedNumber { get; set; }
    
    public string? StandardError { get; set; } = string.Empty;
    
    public string CreatedBy { get; set; } = string.Empty;
    
    public DateTime CreatedAt { get; set; }
    
    public string UpdatedBy { get; set; } = string.Empty;
    
    public DateTime? UpdatedAt { get; set; }
    
    //Relationship
    public SpeciesDto Species { get; set; } = new SpeciesDto();
}