using System.ComponentModel.DataAnnotations;
using conservation_backend.Shared;

namespace conservation_backend.Features.LessActivities;

public class LessActivity : BaseEntity
{
    public Guid Id { get; set; } = Guid.NewGuid();

    [MaxLength(20)]
    public string Category { get; set; } = string.Empty;

    [MaxLength(100)]
    public string Key { get; set; } = string.Empty;

    [MaxLength(255)]
    public string Label { get; set; } = string.Empty;

    public int SortOrder { get; set; }

    public bool IsActive { get; set; } = true;
}