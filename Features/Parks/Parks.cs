using System.ComponentModel.DataAnnotations;
using authentication_engine.Shared.Entities;

namespace authentication_engine.Features.Parks;

public class Park : BaseEntity
{
    public Guid Id { get; set; }

    [MaxLength(255)]
    public string Name { get; set; } = string.Empty;

    [MaxLength(255)]
    public string? Code{ get; set; } = string.Empty;
    
    [MaxLength(5)]
    public string Zone { get; set; } = string.Empty;
    
}
