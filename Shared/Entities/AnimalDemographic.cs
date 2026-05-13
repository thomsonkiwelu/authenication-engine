using System.ComponentModel.DataAnnotations;

namespace conservation_backend.Shared.Entities;

public class AnimalDemographic : BaseEntity
{
    public Guid Id { get; set; }
    
    [MaxLength(50)]
    public string AgeCategory { get; set; } = string.Empty;
    
    [MaxLength(50)]
    public string Sex { get; set; } = string.Empty;
    
    public int Count { get; set; }
    
    public Guid EntityId { get; set; }
        
    [MaxLength(50)]
    public string EntityName { get; set; } = string.Empty;
}