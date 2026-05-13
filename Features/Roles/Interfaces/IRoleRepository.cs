using authentication_engine.Features.Permissions;
using authentication_engine.Shared;

namespace authentication_engine.Features.Roles.Interfaces
{
    public interface IRoleRepository
    {
        Task<PagedList<Role>> GetPagedData(RolePaginationDto dto);

        Task<Role> Create(Role role);

        Task<(Role role, List<PermissionMinimalDto> permissions)> GetById(Guid id);

        Task<Role> Update(Guid id, Role role);

        Task<bool> Delete(Guid id);

        Task<bool> UpdateRolePermissions(AssignRolePermissionRequest dto);
        
        Task<bool> AssignRoleToUser(RoleUser roleUser);
        
        Task<bool> UnassignRoleToUser(RoleUser roleUser);
    }
}
