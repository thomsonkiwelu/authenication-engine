using System.ComponentModel.DataAnnotations;
using authentication_engine.Shared.Entities;

namespace authentication_engine.Features.SystemModules;

public class SystemModule : BaseEntity
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();

    [MaxLength(255)]
    public string Name { get; set; } = string.Empty;

    [MaxLength(255)]
    public string Slug { get; set; } = string.Empty;
}