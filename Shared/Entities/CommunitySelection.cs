using System.ComponentModel.DataAnnotations;

namespace conservation_backend.Shared.Entities;

public class CommunitySelection : BaseEntity
{
    public Guid Id { get; set; }
    
    [MaxLength(150)]
    public string Value { get; set; } = string.Empty;
    
    [MaxLength(150)]
    public string? OtherName { get; set; } = string.Empty;
    
    [MaxLength(150)]
    public string FieldName { get; set; } = string.Empty;
    
    public Guid EntityId { get; set; }
        
    [MaxLength(50)]
    public string EntityName { get; set; } = string.Empty;
}