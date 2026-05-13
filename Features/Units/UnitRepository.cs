using System.Text.Json;
using authentication_engine.Config;
using authentication_engine.Features.Units.Interfaces;
using authentication_engine.Shared;
using Microsoft.EntityFrameworkCore;

namespace authentication_engine.Features.Units
{
    public class UnitRepository(AppDBContext context, IUserContext userContext) : IUnitRepository
    {
        private readonly AppDBContext _context = context;
        private readonly IUserContext _userContext = userContext;

        public async Task<PagedList<Unit>> GetPagedData(UnitPaginationDto dto)
        {
            string[] searchColumns = new string[] { "Name" };
            var query = _context.Units.Include(d => d.Department)
                .Include(d => d.Section)
                .AsNoTracking().AsQueryable();

            //Apply search filter
            query = ApplyFilters<Unit>.ApplySearch(query, dto.q ?? "", searchColumns);

            //Apply sorting filter
            query = ApplyFilters<Unit>.ApplySorting(query, dto.sortBy, dto.sortDesc);
            
            if (!string.IsNullOrWhiteSpace(dto.OfficeId))
                query = query.Where(v => v.OfficeId == Guid.Parse(dto.OfficeId));

            return await PagedList<Unit>.CreateAsync(query, dto.page, dto.pageSize);
        }

        public async Task<Unit> Create(Unit unit)
        {
            unit.CreatedBy = _userContext.GetUserId();
            _context.Units.Add(unit);

            await _context.SaveChangesAsync();
            return unit;
        }

        public async Task<Unit> GetById(Guid id)
        {
            var unit = await _context.Units.FindAsync(id);

            if (unit is null)
                throw new KeyNotFoundException($"Unit record not found");

            return unit;
        }

        public async Task<Unit> Update(Guid id, Unit unit)
        {
            var rows = await _context.Database.ExecuteSqlInterpolatedAsync($@"
                UPDATE ""Units""
                SET ""Name"" = {unit.Name},""UpdatedBy"" = {_userContext.GetUserId()},""OfficeId"" = {unit.OfficeId},
                 ""DepartmentId"" = {unit.DepartmentId},""Code"" = {unit.Code},""SectionId"" = {unit.SectionId},
                ""UpdatedAt"" = {DateTime.UtcNow} WHERE ""Id"" = {id};
            ");
            
            if (rows == 0)
                throw new KeyNotFoundException("Unit record not found.");
            
            var updatedUnit = await _context.Units.FindAsync(id);
            return updatedUnit ?? throw new KeyNotFoundException("Failed to retrieve updated record");
        }

        public async Task<bool> Delete(Guid id)
        {
            var unit = await _context.Units.FindAsync(id);

            if (unit is null)
                throw new KeyNotFoundException("Unit record not found");

            unit.DeletedAt = DateTime.UtcNow;
            unit.UpdatedBy = _userContext.GetUserId();
            unit.UpdatedAt = DateTime.UtcNow;
          
            await _context.SaveChangesAsync();
            return true;
        }
        
        public async Task<UnitsWithDepartmentAndSectionDto> GetUnitsWithDepartmentAndSection(Guid officeId)
        {
            var dataSql = "SELECT * FROM fun_get_units_departments_sections({0})";
            var results = await _context.Database.SqlQueryRaw<string>(dataSql, officeId)
                .ToListAsync();

            var jsonString = results.FirstOrDefault();
            if (string.IsNullOrWhiteSpace(jsonString)) return new UnitsWithDepartmentAndSectionDto();

            var result = JsonSerializer.Deserialize<UnitsWithDepartmentAndSectionDto>(
                jsonString,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
            );
            return result ?? new UnitsWithDepartmentAndSectionDto();
        }
    }
}
