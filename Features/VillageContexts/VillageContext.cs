using System.ComponentModel.DataAnnotations;
using conservation_backend.Shared;

namespace conservation_backend.Features.VillageContexts;

public class VillageContext: BaseEntity
{
    public Guid Id { get; set; }
    
    [MaxLength(250)]
    public string Name { get; set; } = string.Empty;
    
    [MaxLength(250)]
    public string Data { get; set; } = string.Empty;
    
    [MaxLength(255)]
    public string? Description { get; set; } = string.Empty;
    
    public Guid EntityId { get; set; }
    
    [MaxLength(50)]
    public string EntityName { get; set; } = string.Empty;
    
    [MaxLength(50)]
    public string FieldName { get; set; } = string.Empty;
}