using System.ComponentModel.DataAnnotations;
using conservation_backend.Shared;

namespace conservation_backend.Features.GovernmentLeaders;

public class GovernmentLeader: BaseEntity
{
    public Guid Id { get; set; }
    
    [MaxLength(50)]
    public string FullName { get; set; } = string.Empty;
    
    [MaxLength(100)]
    public string Position { get; set; } = string.Empty;
    
    [MaxLength(15)]
    public string Mobile { get; set; } = string.Empty;
    
    [MaxLength(20)]
    public string? TelephoneNumber { get; set; } = string.Empty;
    
    [MaxLength(20)]
    public string? Address { get; set; } = string.Empty;
    
    public Guid EntityId { get; set; }
    
    [MaxLength(50)]
    public string EntityName { get; set; } = string.Empty;
    
    [MaxLength(50)]
    public string FieldName { get; set; } = string.Empty;
}