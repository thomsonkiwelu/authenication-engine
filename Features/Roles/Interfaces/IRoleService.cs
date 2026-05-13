using conservation_backend.Shared;

namespace conservation_backend.Features.Roles.Interfaces
{
    public interface IRoleService
    {
        Task<PagedList<RoleResponseDto>> GetAllRolesData(PaginationDto dto);

        Task<RoleDto> CreateRoles(RoleRequest dto);

        Task<RoleWithPermissionsDto> GetRolesById(Guid id);

        Task<RoleDto> UpdateRoles(Guid id, RoleRequest dto);

        Task<bool> DeleteRoles(Guid id);

        Task<bool> UpdateRolePermissions(AssignRolePermissionRequest dto);
        
        Task<bool> AssignRoleToUser(AssignRoleToUserRequest dto);
        
        Task<bool> UnassignRoleToUser(AssignRoleToUserRequest dto);
    }
}
