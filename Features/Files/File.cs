using System.ComponentModel.DataAnnotations;
using conservation_backend.Shared;

namespace conservation_backend.Features.Files;

public class FileEntity: BaseEntity
{
    public Guid Id { get; set; }
    
    [MaxLength(250)]
    public string FileName { get; set; } = string.Empty;

    [MaxLength(250)]
    public string FilePath { get; set; } = string.Empty;

    [MaxLength(250)]
    public string FileType { get; set; } = string.Empty;

    public long FileSize { get; set; }
    
    public Guid EntityId { get; set; }
    
    [MaxLength(50)]
    public string EntityName { get; set; } = string.Empty;
}