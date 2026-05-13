using authentication_engine.Shared;

namespace authentication_engine.Features.Roles.Interfaces
{
    public interface IRoleService
    {
        Task<PagedList<RoleResponseDto>> GetAllRolesData(RolePaginationDto dto);

        Task<RoleDto> CreateRoles(RoleRequest dto);

        Task<RoleWithPermissionsDto> GetRolesById(Guid id);

        Task<RoleDto> UpdateRoles(Guid id, RoleRequest dto);

        Task<bool> DeleteRoles(Guid id);

        Task<bool> UpdateRolePermissions(AssignRolePermissionRequest dto);
        
        Task<bool> AssignRoleToUser(AssignRoleToUserRequest dto);
        
        Task<bool> UnassignRoleToUser(AssignRoleToUserRequest dto);
    }
}
