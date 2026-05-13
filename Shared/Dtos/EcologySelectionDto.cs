namespace conservation_backend.Shared.Dtos;

public class EcologySelectionDto
{
    public Guid Id { get; set; }
    
    public string Value { get; set; } = string.Empty;
    
    public string FieldName { get; set; } = string.Empty;
    
    public Guid EntityId { get; set; }
    
    public string EntityName { get; set; } = string.Empty;
    
    public string CreatedBy { get; set; } = string.Empty;
    
    public DateTime CreatedAt { get; set; }
    
    public string UpdatedBy { get; set; } = string.Empty;
    
    public DateTime? UpdatedAt { get; set; }
}