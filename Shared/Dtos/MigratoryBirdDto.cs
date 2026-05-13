using conservation_backend.Features.Files;
using conservation_backend.Features.Species;

namespace conservation_backend.Shared.Dtos;

public class MigratoryBirdDto
{
    public Guid Id { get; set; }
    
    public Guid SpeciesId { get; set; }

    public string MigrationType { get; set; } = string.Empty;

    public string Arrival { get; set; } = string.Empty;

    public string Activity { get; set; } = string.Empty;
    
    public int IndividualObserved { get; set; }

    public string Remark { get; set; } = string.Empty;
    
    public string Category { get; set; } = string.Empty;
    
    public Guid EntityId { get; set; }
    
    public string EntityName { get; set; } = string.Empty;
    
    public string CreatedBy { get; set; } = string.Empty;
    
    public DateTime CreatedAt { get; set; }
    
    public string UpdatedBy { get; set; } = string.Empty;
    
    public DateTime? UpdatedAt { get; set; }
    
    //Relationship
    public SpeciesDto Species { get; set; } = new SpeciesDto();
    public FileDto? File { get; set; } = new FileDto();
}