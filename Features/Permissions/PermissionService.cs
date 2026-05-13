using conservation_backend.Features.Permissions.Interfaces;
using conservation_backend.Shared;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;

namespace conservation_backend.Features.Permissions
{
    public class PermissionService(IPermissionRepository repository, IMapper mapper) : IPermissionService
    {
        private readonly IPermissionRepository _permissionRepository = repository;

        private readonly IMapper _mapper = mapper;

        public async Task<PagedList<PermissionResponseDto>> GetAllPermissionsData(PermissionPaginationDto dto)
        {
            var pagedData = await _permissionRepository.GetPagedData(dto);

            var startIndex = (pagedData.Page - 1) * pagedData.PageSize + 1;

            var dtoList = _mapper.Map<List<PermissionResponseDto>>(pagedData.Data)
                .Select((dto, index) => dto with { RowNumber = startIndex + index }).ToList();

            return new PagedList<PermissionResponseDto>(
                items: dtoList,
                page: pagedData.Page,
                pageSize: pagedData.PageSize,
                totalCount: pagedData.TotalCount
            );
        }

        public async Task<PermissionDto> CreatePermission(PermissionRequest dto)
        {
            var permission = _mapper.Map<PermissionEntity>(dto);

            var createdPermission = await _permissionRepository.Create(permission);

            var responseDto = _mapper.Map<PermissionDto>(createdPermission);

            return responseDto;
        }

        public async Task<PermissionDto> GetPermissionById(Guid id)
        {
            var role = await _permissionRepository.GetById(id);

            var result = _mapper.Map<PermissionDto>(role);

            return result;
        }

        public async Task<PermissionDto> UpdatePermission(Guid id, PermissionRequest dto)
        {
            var permission = _mapper.Map<PermissionEntity>(dto);

            var updatedPermission = await _permissionRepository.Update(id, permission);

            var responseDto = _mapper.Map<PermissionDto>(updatedPermission);

            return responseDto;
        }

        public async Task<bool> DeletePermission(Guid id)
        {
            return await _permissionRepository.Delete(id);
        }

        public async Task<List<PermissionGroupDto>> GetPermissionsGroupedBySystemModel(string slugName)
        {
            return await _permissionRepository.GetPermissionsGroupedBySystemModel(slugName); 
        }

        public async Task<List<string>> GetAllSystemModels()
        {
            return await _permissionRepository.GetSystemModels();
        }
        
    }
}
