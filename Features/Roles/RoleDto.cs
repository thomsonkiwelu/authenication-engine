using authentication_engine.Features.Permissions;
using authentication_engine.Features.SystemApplications;
using authentication_engine.Features.Users;
using authentication_engine.Shared;

namespace authentication_engine.Features.Roles
{ 
    public record RolePaginationDto : PaginationDto
    {
        public string? SystemApplicationId { get; init; }
    }
    
    public record RoleRequest
    {
        public string Name { get; init; } = string.Empty;
        public string Slug { get; init; } = string.Empty;
        public string Description { get; init; } = string.Empty;
        public string SystemApplicationId { get; init; } = string.Empty;
        public Guid CreatedBy { get; set; }
        public Guid? UpdatedBy { get; set; }
    }

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
        public string SystemApplicationId { get; init; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; } = string.Empty;
        public DateTime? UpdatedAt { get; set; }
        public string UpdatedBy { get; set; } = string.Empty;
        //Relationship
        public SystemApplicationDto SystemApplication { get; set; } = new SystemApplicationDto();
        public UserMinimalDto Creator { get; set; } = new UserMinimalDto();
        public UserMinimalDto Updater { get; set; } = new UserMinimalDto();
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
