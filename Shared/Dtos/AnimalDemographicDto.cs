namespace conservation_backend.Shared.Dtos;

public class AnimalDemographicDto
{
    public Guid Id { get; set; }
    
    public string AgeCategory { get; set; } = string.Empty;

    public string Sex { get; set; } = string.Empty;
    
    public int Count { get; set; }
    
    public Guid EntityId { get; set; }

    public string EntityName { get; set; } = string.Empty;
    
    public string CreatedBy { get; set; } = string.Empty;
    
    public DateTime CreatedAt { get; set; }
    
    public string UpdatedBy { get; set; } = string.Empty;
    
    public DateTime? UpdatedAt { get; set; }
}