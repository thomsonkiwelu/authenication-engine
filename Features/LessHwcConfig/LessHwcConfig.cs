using System.ComponentModel.DataAnnotations;
using conservation_backend.Shared;

namespace conservation_backend.Features.LessHwcConfig;

public class LessHwcTabDefinition : BaseEntity
{
    public Guid Id { get; set; } = Guid.NewGuid();

    [MaxLength(50)]
    public string Key { get; set; } = string.Empty;

    [MaxLength(255)]
    public string Label { get; set; } = string.Empty;

    public int SortOrder { get; set; }

    public bool IsActive { get; set; } = true;

    public ICollection<LessHwcFieldDefinition> Fields { get; set; } = new List<LessHwcFieldDefinition>();
}

public class LessHwcFieldDefinition : BaseEntity
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public Guid TabDefinitionId { get; set; }

    public LessHwcTabDefinition TabDefinition { get; set; } = null!;

    [MaxLength(100)]
    public string Key { get; set; } = string.Empty;

    [MaxLength(255)]
    public string Label { get; set; } = string.Empty;

    [MaxLength(50)]
    public string FieldType { get; set; } = "text"; // text, number, select, multiselect, textarea, date

    [MaxLength(255)]
    public string Placeholder { get; set; } = string.Empty;

    public bool IsRequired { get; set; }

    public bool IsComputed { get; set; }

    [MaxLength(1000)]
    public string ComputeFromKeys { get; set; } = string.Empty; // comma-separated keys

    public int SortOrder { get; set; }

    public bool IsActive { get; set; } = true;

    public ICollection<LessHwcFieldOption> Options { get; set; } = new List<LessHwcFieldOption>();
}

public class LessHwcFieldOption : BaseEntity
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public Guid FieldDefinitionId { get; set; }

    public LessHwcFieldDefinition FieldDefinition { get; set; } = null!;

    [MaxLength(100)]
    public string Value { get; set; } = string.Empty;

    [MaxLength(255)]
    public string Label { get; set; } = string.Empty;

    public int SortOrder { get; set; }

    public bool IsActive { get; set; } = true;
}
