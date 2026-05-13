using System.ComponentModel.DataAnnotations;
using conservation_backend.Features.DistrictProfiles;
using conservation_backend.Shared;

namespace conservation_backend.Features.DistrictContexts;

public class DistrictContext : BaseEntity
{
    public Guid Id { get; set; }
    
    [MaxLength(250)]
    public string Name { get; set; } = string.Empty;
    
    [MaxLength(255)]
    public string? Description { get; set; } = string.Empty;
    
    public Guid DistrictProfileId { get; set; }
    public DistrictProfile DistrictProfile { get; set; } = null!;
    
    [MaxLength(50)]
    public string FieldName { get; set; } = string.Empty;
}

public class DevelopmentOrganization : BaseEntity
{
    public Guid Id { get; set; }
    
    [MaxLength(250)]
    public string Name { get; set; } = string.Empty;
    
    [MaxLength(150)]
    public string TimeOfOperation { get; set; } = string.Empty;
    
    [MaxLength(150)]
    public string Address { get; set; } = string.Empty;
    
    [MaxLength(20)]
    public string TelephoneNumber { get; set; } = string.Empty;
    
    [MaxLength(50)]
    public string ContactPersonName { get; set; } = string.Empty;
    
    [MaxLength(20)]
    public string ContactPersonMobile { get; set; } = string.Empty;
    
    [MaxLength(150)]
    public string AreaOfOperation { get; set; } = string.Empty;
    
    [MaxLength(150)]
    public string TypeOfOperation { get; set; } = string.Empty;
    
    [MaxLength(250)]
    public string Comment { get; set; } = string.Empty;
    
    public Guid DistrictProfileId { get; set; }
    public DistrictProfile DistrictProfile { get; set; } = null!;
    
}