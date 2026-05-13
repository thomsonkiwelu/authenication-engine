using conservation_backend.Shared;

namespace conservation_backend.Features.Permissions.Interfaces
{
    public interface IPermissionRepository
    {
        Task<PagedList<PermissionEntity>> GetPagedData(PermissionPaginationDto dto);

        Task<PermissionEntity> Create(PermissionEntity permission);

        Task<PermissionEntity> GetById(Guid id);

        Task<PermissionEntity> Update(Guid id, PermissionEntity species);

        Task<bool> Delete(Guid id);

        Task<List<PermissionGroupDto>> GetPermissionsGroupedBySystemModel(string slugName);

        Task<List<string>> GetSystemModels();
    }
}
