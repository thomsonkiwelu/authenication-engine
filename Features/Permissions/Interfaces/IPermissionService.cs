using authentication_engine.Shared;

namespace authentication_engine.Features.Permissions.Interfaces
{
    public interface IPermissionService
    {
        Task<PagedList<PermissionResponseDto>> GetAllPermissionsData(PermissionPaginationDto dto);

        Task<PermissionDto> CreatePermission(PermissionRequest dto);

        Task<PermissionDto> GetPermissionById(Guid id);

        Task<PermissionDto> UpdatePermission(Guid id, PermissionRequest dto);

        Task<bool> DeletePermission(Guid id);

        Task<List<PermissionGroupDto>> GetPermissionsGroupedBySystemModel(string slugName);

        Task<List<string>> GetAllSystemModels();
    }
}
