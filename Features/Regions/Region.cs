using System.ComponentModel.DataAnnotations;
using conservation_backend.Shared;

namespace conservation_backend.Features.Regions;

public class Region: BaseEntity
{
    public Guid Id { get; set; }
    
    [MaxLength(50)]
    public string Name { get; set; } =  string.Empty;
}