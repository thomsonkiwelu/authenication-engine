using conservation_backend.Features.Permissions;
using conservation_backend.Shared;

namespace conservation_backend.Features.Roles.Interfaces
{
    public interface IRoleRepository
    {
        Task<PagedList<Role>> GetPagedData(PaginationDto dto);

        Task<Role> Create(Role role);

        Task<(Role role, List<PermissionMinimalDto> permissions)> GetById(Guid id);

        Task<Role> Update(Guid id, Role role);

        Task<bool> Delete(Guid id);

        Task<bool> UpdateRolePermissions(AssignRolePermissionRequest dto);
        
        Task<bool> AssignRoleToUser(RoleUser roleUser);
        
        Task<bool> UnassignRoleToUser(RoleUser roleUser);
    }
}
