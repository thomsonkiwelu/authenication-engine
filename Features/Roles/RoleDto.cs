using authentication_engine.Features.Permissions;
using authentication_engine.Features.SystemApplications;
using authentication_engine.Features.SystemModules;
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
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Slug { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string SystemApplicationId { get; set; } = string.Empty;
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
        public List<PermissionMinimalDto> Permissions { get; set; } = new List<PermissionMinimalDto>();
        public List<SystemModuleMinimalDto> SystemModules { get; set; } = new List<SystemModuleMinimalDto>();
    }
    
    public record RoleResponseDto : RoleDto
    {
        public int RowNumber { get; set; }
        
        public string SystemModuleSlugName { get; set; } = string.Empty;
    }
    
    public record AssignRoleToUserRequest(
        Guid RoleId,
        Guid UserId,
        String? CreatedBy
    );

    public record RoleSqlResponseDto
    {
        public List<RoleResponseDto> Data { get; set; } = new();
        public PaginationMeta Meta { get; init; } = new ();
    }
    
    public record RoleMinimalDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Slug { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
    }

}
