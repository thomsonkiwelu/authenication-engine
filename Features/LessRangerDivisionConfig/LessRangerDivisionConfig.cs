using System.ComponentModel.DataAnnotations;
using conservation_backend.Features.Ranks;
using conservation_backend.Shared;

namespace conservation_backend.Features.LessRangerDivisionConfig;

public class LessRangerRankCategory : BaseEntity
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public Guid RankId { get; set; }

    public Rank Rank { get; set; } = null!;

    [MaxLength(20)]
    public string Category { get; set; } = string.Empty;
}

public class LessRangerDutyFieldDefinition : BaseEntity
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
