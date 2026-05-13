namespace conservation_backend.Features.Files;

public record UploadFileDto
{
    public IFormFile File { get; set; } = null!;
}

public record FileDto
{
    public Guid Id { get; set; }
    public string FileName { get; set; } = string.Empty;
    public string FilePath { get; set; } = string.Empty;
    public string FileType { get; set; } = string.Empty;
    public long FileSize { get; set; }
    public string? FileUrl { get; set; } = string.Empty;
    public Guid EntityId { get; set; }
    public string EntityName { get; set; } = string.Empty;
    public string CreatedBy { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}

public record UpdateFileDto
{
    public IFormFile File { get; set; } = null!;
    public Guid EntityId { get; set; }
    public string EntityName { get; set; } = string.Empty;
    public string? Id { get; set; } = string.Empty;
}

