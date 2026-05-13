using System.ComponentModel.DataAnnotations;
using conservation_backend.Shared;

namespace conservation_backend.Features.LessLivestockConfig;

public class LessLivestockType : BaseEntity
{
    public Guid Id { get; set; } = Guid.NewGuid();

    [MaxLength(50)]
    public string Key { get; set; } = string.Empty;

    [MaxLength(255)]
    public string Label { get; set; } = string.Empty;

    public int SortOrder { get; set; }

    public bool IsActive { get; set; } = true;
}

public class LessLivestockActionOption : BaseEntity
{
    public Guid Id { get; set; } = Guid.NewGuid();

    [MaxLength(50)]
    public string Key { get; set; } = string.Empty;

    [MaxLength(255)]
    public string Label { get; set; } = string.Empty;

    public int SortOrder { get; set; }

    public bool RequiresControlNumber { get; set; }

    public bool RequiresCaseNumber { get; set; }

    public bool IsRevenue { get; set; }

    public bool IsActive { get; set; } = true;
}
