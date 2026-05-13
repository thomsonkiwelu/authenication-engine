using conservation_backend.Features.SystemModules;
using conservation_backend.Shared;

namespace conservation_backend.Features.Permissions
{
    public record PermissionPaginationDto : PaginationDto
    {
        public string? module { get; init; }
    }

    public record PermissionDto(
       Guid Id,
       string Name,
       string Action,
       string ModelType,
       string SystemModuleId,
       DateTime CreatedAt
    );

    public record PermissionRequest(
        string Name,
        string Action,
        string ModelType
    );
        
    public record PermissionResponseDto()
    {
        public Guid Id { get; init; }

        public int RowNumber { get; init; }
        
        public string Name { get; init; } = string.Empty;
        
        public string Action { get; init; } = string.Empty;
        
        public string ModelType { get; init; } = string.Empty;

        public string SystemModuleId { get; init; } = string.Empty;
        
        public DateTime CreatedAt { get; init; }
        
        public SystemModuleDto SystemModule { get; init; }
    }

    public record PermissionGroupDto
    {
        public string ModelType { get; init; } = string.Empty;

        public List<PermissionMinimalDto> Permissions { get; init; } = new List<PermissionMinimalDto>();
    }

    public record PermissionMinimalDto()
    {
        public Guid Id { get; init; }

        public string Name { get; init; } = string.Empty;

        public string Action { get; init; } = string.Empty;
    }

}
