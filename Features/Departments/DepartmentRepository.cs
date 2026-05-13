using System.Text.Json;
using conservation_backend.Config;
using conservation_backend.Features.Departments.Interfaces;
using conservation_backend.Shared;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using NpgsqlTypes;

namespace conservation_backend.Features.Departments
{
    public class DepartmentRepository(AppDBContext context, IUserContext userContext) : IDepartmentRepository
    {
        private readonly AppDBContext _context = context;
        private readonly IUserContext _userContext = userContext;
        
        public async Task<PagedList<Department>> GetPagedData(DepartmentPaginationDto dto)
        {
            string[] searchColumns = new string[] { "Name" };
            var query = _context.Departments.AsNoTracking().AsQueryable();

            //Apply search filter
            query = ApplyFilters<Department>.ApplySearch(query, dto.q ?? "", searchColumns);

            //Apply sorting filter
            query = ApplyFilters<Department>.ApplySorting(query, dto.sortBy, dto.sortDesc);

            if (!string.IsNullOrWhiteSpace(dto.OfficeId))
                query = query.Where(v => v.OfficeId == Guid.Parse(dto.OfficeId));

            return await PagedList<Department>.CreateAsync(query, dto.page, dto.pageSize);
        }

        public async Task<Department> Create(Department department)
        {
            department.CreatedBy = _userContext.GetUserId();
            _context.Departments.Add(department);

            await _context.SaveChangesAsync();
            return department;
        }

        public async Task<Department> GetById(Guid id)
        {
            var department = await _context.Departments
                .Include(s => s.Office)
                .FirstOrDefaultAsync(s => s.Id == id);

            if (department is null)
                throw new KeyNotFoundException($"Department record not found");

            return department;
        }

        public async Task<Department> Update(Guid id, Department department)
        {
            var rows = await _context.Database.ExecuteSqlInterpolatedAsync($@"
                UPDATE ""Departments""
                SET ""Name"" = {department.Name}, ""Code"" = {department.Code},""UpdatedBy"" = {_userContext.GetUserId()},
                ""OfficeId"" = {department.OfficeId},""UpdatedAt"" = {DateTime.UtcNow} WHERE ""Id"" = {id};
            ");
            
            if (rows == 0)
                throw new KeyNotFoundException("Department record not found.");
            
            var updatedDepartment = await _context.Departments
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);
            
            return updatedDepartment ?? throw new KeyNotFoundException("Failed to retrieve updated record");
        }

        public async Task<bool> Delete(Guid id)
        {
            var department = await _context.Departments.FindAsync(id);
            
            if (department is null)
                throw new KeyNotFoundException("Department record not found");

            department.DeletedAt = DateTime.UtcNow;
            department.UpdatedBy = _userContext.GetUserId();
            department.UpdatedAt = DateTime.UtcNow;
            
            await _context.SaveChangesAsync();
            return true;
        }
        
        public async Task<bool> AssignStaffToDepartment(AssignStaffToDepartmentDto dto)
        {
            dto.CreatedBy = _userContext.GetUserId();
            var dtoToJson = JsonSerializer.Serialize(dto);
            
            await using var connection = new NpgsqlConnection(_context.Database.GetConnectionString());
            await connection.OpenAsync();
            await using var command = new NpgsqlCommand(
                "SELECT fun_assign_staff_to_department(@data::jsonb)",
                connection
            );
            command.Parameters.AddWithValue("data", NpgsqlDbType.Jsonb, dtoToJson);
            var result = await command.ExecuteScalarAsync();
            
            if (result is null) return false;
            return (bool)result;
        }
        
        public async Task<bool> UnassignStaffFromDepartment(UnassignStaffFromDepartmentDto dto)
        {
            dto.CurrentUser = _userContext.GetUserId();
            var dtoToJson = JsonSerializer.Serialize(dto);
            
            await using var connection = new NpgsqlConnection(_context.Database.GetConnectionString());
            await connection.OpenAsync();
            await using var command = new NpgsqlCommand(
                "SELECT fun_unassign_staff_from_department(@data::jsonb)",
                connection
            );
            command.Parameters.AddWithValue("data", NpgsqlDbType.Jsonb, dtoToJson);
            var result = await command.ExecuteScalarAsync();
            
            if (result is null) return false;
            return (bool)result;
        }
        
        public async Task<bool> UpdateAssignStaffDepartment(DepartmentStaff departmentStaffs)
        {
            var rows = await _context.Database.ExecuteSqlInterpolatedAsync($@"
                UPDATE ""DepartmentStaffs""
                SET ""DepartmentId"" = {departmentStaffs.DepartmentId},""ModelType"" = {departmentStaffs.ModelType},""UpdatedBy"" = {_userContext.GetUserId()},
                    ""UpdatedAt"" = {DateTime.UtcNow}
                WHERE ""StaffId"" = {departmentStaffs.StaffId};
            ");
            
            if (rows == 0) return false;
            
            return true;
        }

    }
}
