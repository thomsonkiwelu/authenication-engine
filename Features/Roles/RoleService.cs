using authentication_engine.Features.Roles.Interfaces;
using authentication_engine.Features.SystemApplications;
using authentication_engine.Shared;
using MapsterMapper;

namespace authentication_engine.Features.Roles
{
    public class RoleService(IRoleRepository repository, IMapper mapper) : IRoleService
    {
        private readonly IRoleRepository _roleRepository = repository;

        private readonly IMapper _mapper = mapper;

        public async Task<PagedList<RoleResponseDto>> GetAllRolesData(RolePaginationDto dto)
        {
            var pagedData = await _roleRepository.GetPagedData(dto);
        
            return new PagedList<RoleResponseDto>(
                items: pagedData.Data,
                page: pagedData.Page,
                pageSize: pagedData.PageSize,
                totalCount: pagedData.TotalCount
            );
        }

        public async Task<RoleDto> CreateRoles(RoleRequest dto)
        {
            var role = _mapper.Map<Role>(dto);

            var createdRoles = await _roleRepository.Create(role);

            var responseDto = _mapper.Map<RoleDto>(createdRoles);

            return responseDto;
        }

        public async Task<RoleWithPermissionsDto> GetRolesById(Guid id)
        {
            var (role, permissions, systemModules) = await _roleRepository.GetById(id);

            return new RoleWithPermissionsDto
            {
                Id = role.Id,
                Name = role.Name,
                Description = role.Description,
                CreatedAt = role.CreatedAt,
                Permissions = permissions,
                SystemModules = systemModules,
                SystemApplication = role.SystemApplication != null ? new SystemApplicationDto
                {
                    Id = role.SystemApplication.Id,
                    Name = role.SystemApplication.Name
                } : new SystemApplicationDto(),
            };
        }

        public async Task<RoleDto> UpdateRoles(Guid id, RoleRequest dto)
        {
            var role = _mapper.Map<Role>(dto);

            var updatedRoles = await _roleRepository.Update(id, role);

            var responseDto = _mapper.Map<RoleDto>(updatedRoles);

            return responseDto;
        }

        public async Task<bool> DeleteRoles(Guid id)
        {
            return await _roleRepository.Delete(id);
        }

        public async Task<bool> UpdateRolePermissions(AssignRolePermissionRequest dto)
        {
            return await _roleRepository.UpdateRolePermissions(dto);
        }
        
        public async Task<bool> AssignRoleToUser(AssignRoleToUserRequest dto)
        {
            var roleUser = _mapper.Map<RoleUser>(dto);
            
            return await _roleRepository.AssignRoleToUser(roleUser);
        }
        
        public async Task<bool> UnassignRoleToUser(AssignRoleToUserRequest dto)
        {
            var roleUser = _mapper.Map<RoleUser>(dto);
            
            return await _roleRepository.UnassignRoleToUser(roleUser);
        }
    }
}
