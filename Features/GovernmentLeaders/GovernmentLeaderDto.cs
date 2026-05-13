using conservation_backend.Features.Users;
using conservation_backend.Shared;

namespace conservation_backend.Features.GovernmentLeaders;

public record GovernmentLeaderPaginationDto : PaginationDto
{
    public string? EntityId { get; set; }
    
    public string? EntityName { get; set; }
}

public record GovernmentLeaderDto
{
    public Guid Id { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string Position { get; set; } = string.Empty;
    public string Mobile { get; set; } = string.Empty;
    public string? TelephoneNumber { get; set; } = string.Empty;
    public string? Address { get; set; } = string.Empty;
    public Guid EntityId { get; set; }
    public string EntityName { get; set; } = string.Empty;
    public string FieldName { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public string CreatedBy { get; set; } = string.Empty;
    public DateTime? UpdatedAt { get; set; }
    public string UpdatedBy { get; set; } = string.Empty;
    //Relationship
    public UserMinimalDto Creator { get; set; } = new UserMinimalDto();
    public UserMinimalDto Updater { get; set; } = new UserMinimalDto();
}

public record GovernmentLeaderRequestDto
{
    public string FullName { get; set; } = string.Empty;
    public string Position { get; set; } = string.Empty;
    public string Mobile { get; set; } = string.Empty;
    public string? TelephoneNumber { get; set; } = string.Empty;
    public string? Address { get; set; } = string.Empty;
    public Guid EntityId { get; set; }
    public string EntityName { get; set; } = string.Empty;
    public string FieldName { get; set; } = string.Empty;
    public Guid CreatedBy { get; set; }
    public Guid? UpdatedBy { get; set; }
}

public record GovernmentLeaderResponseDto : GovernmentLeaderDto
{
    public int RowNumber { get; set; }
}