using authentication_engine.Features.Permissions;

namespace authentication_engine.Features.Roles
{ 
    public record RoleRequest(
        string Name,
        string Slug,
        string Description
    );

    public record AssignRolePermissionRequest(
        Guid RoleId,
        List<string> PermissionIds,
        string ModuleName
    );

    public record RoleDto
    {
        public Guid Id { get; init; }

        public string Name { get; init; } = string.Empty;
        
        public string Slug { get; init; } = string.Empty;

        public string Description { get; init; } = string.Empty;

        public DateTime CreatedAt { get; init; }
        
        public string CreatedBy { get; init; }
    }

    public record RoleWithPermissionsDto : RoleDto
    {
        public List<PermissionMinimalDto> Permissions { get; init; } = new List<PermissionMinimalDto>();
    }
    
    public record RoleResponseDto : RoleDto
    {
        public int RowNumber { get; init; }
    }
    
    public record AssignRoleToUserRequest(
        Guid RoleId,
        Guid UserId,
        String? CreatedBy
    );

}
