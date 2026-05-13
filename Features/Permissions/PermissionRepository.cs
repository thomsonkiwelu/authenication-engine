using authentication_engine.Config;
using authentication_engine.Features.Permissions.Interfaces;
using authentication_engine.Shared;
using Microsoft.EntityFrameworkCore;

namespace authentication_engine.Features.Permissions
{
    public class PermissionRepository(AppDBContext context, IUserContext userContext) : IPermissionRepository
    {
        private readonly AppDBContext _context = context;
        
        private readonly IUserContext _userContext = userContext;

        public async Task<PagedList<PermissionEntity>> GetPagedData(PermissionPaginationDto dto)
        {
            string[] searchColumns = new string[] { "ModelType" };
            var query = _context.Permissions
                .Include(s => s.SystemModule).AsNoTracking().AsQueryable();

            //Apply search filter
            query = ApplyFilters<PermissionEntity>.ApplySearch(query, dto.q ?? "", searchColumns);

            //Apply sorting filter
            query = ApplyFilters<PermissionEntity>.ApplySorting(query, dto.sortBy, dto.sortDesc);

            if (!string.IsNullOrWhiteSpace(dto.module))
                query = query.Where(v => v.SystemModuleId == Guid.Parse(dto.module));

            return await PagedList<PermissionEntity>.CreateAsync(query, dto.page, dto.pageSize);
        }

        public async Task<PermissionEntity> Create(PermissionEntity permission)
        {
            var permissionModelType = await _context.Permissions.Where(p => p.ModelType == permission.ModelType)
                    .FirstOrDefaultAsync();
            
            if (permissionModelType is null)
                throw new KeyNotFoundException($"Permission model type not found");
            
            permission.CreatedBy = _userContext.GetUserId();
            permission.SystemModuleId = permissionModelType.SystemModuleId;
            
            _context.Permissions.Add(permission);
            await _context.SaveChangesAsync();

            return permission;
        }

        public async Task<PermissionEntity> GetById(Guid id)
        {
            var permission = await _context.Permissions.FindAsync(id);

            if (permission is null)
                throw new KeyNotFoundException($"Permission record not found");

            return permission;
        }

        public async Task<PermissionEntity> Update(Guid id, PermissionEntity permission)
        {
            var existingPermission = await _context.Permissions.FindAsync(id);

            if (existingPermission is null)
                throw new KeyNotFoundException($"Permission record not found");
            
            var permissionModelType = await _context.Permissions.Where(p => p.ModelType == permission.ModelType)
                .FirstOrDefaultAsync();
            
            if (permissionModelType is null)
                throw new KeyNotFoundException($"Permission model type not found");

            permission.Id = existingPermission.Id;
            permission.CreatedAt = existingPermission.CreatedAt;
            permission.CreatedBy = existingPermission.CreatedBy;
            permission.SystemModuleId = permissionModelType.SystemModuleId;
            permission.UpdatedBy = _userContext.GetUserId();
            permission.UpdatedAt = DateTime.UtcNow;
            
            _context.Entry(existingPermission).CurrentValues.SetValues(permission);
            await _context.SaveChangesAsync();

            return existingPermission;
        }

        public async Task<bool> Delete(Guid id)
        {
            var permission = await _context.Permissions.FindAsync(id);

            if (permission is null)
                throw new KeyNotFoundException("Permission record not found");
            
            permission.DeletedAt = DateTime.UtcNow;
            permission.UpdatedBy = _userContext.GetUserId();
            permission.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<List<PermissionGroupDto>> GetPermissionsGroupedBySystemModel(string slugName)
        {
            var systemModule = await context.SystemModules.FirstOrDefaultAsync(m => m.Slug == slugName);

            if (systemModule is null)
                throw new KeyNotFoundException($"System module not found for slug '{slugName}'.");
            
            var query = _context.Permissions.AsQueryable();
            query = query.Where(p => p.SystemModuleId == systemModule.Id);

            var permissionByModelType = await query
                .GroupBy(p => p.ModelType)
                .Select(g => new PermissionGroupDto
                {
                    ModelType = g.Key,
                    Permissions = g.OrderBy(p => p.Name)
                        .Select(p => new PermissionMinimalDto
                        {
                            Id = p.Id,
                            Name = p.Name,
                            Action = p.Action
                        })
                    .ToList()
                })
                .OrderBy(g => g.ModelType)
                .ToListAsync();

            return permissionByModelType;
        }
        
        public async Task<List<string>> GetSystemModels()
        {
            return await _context.Permissions
                .Select(p => p.ModelType)
                .Distinct()
                .OrderBy(modelType => modelType)
                .ToListAsync();
        }

    }
}
