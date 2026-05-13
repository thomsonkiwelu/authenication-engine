using System.Text.Json;
using authentication_engine.Config;
using authentication_engine.Features.Sections.Interfaces;
using authentication_engine.Shared;
using Microsoft.EntityFrameworkCore;

namespace authentication_engine.Features.Sections
{
    public class SectionRepository(AppDBContext context, IUserContext userContext) : ISectionRepository
    {
        private readonly AppDBContext _context = context;
        private readonly IUserContext _userContext = userContext;

        public async Task<PagedList<Section>> GetPagedData(SectionPaginationDto dto)
        {
            string[] searchColumns = new string[] { "Name" };
            var query = _context.Sections.Include(d => d.Department)
                .AsNoTracking().AsQueryable();

            //Apply search filter
            query = ApplyFilters<Section>.ApplySearch(query, dto.q ?? "", searchColumns);

            //Apply sorting filter
            query = ApplyFilters<Section>.ApplySorting(query, dto.sortBy, dto.sortDesc);

            if (!string.IsNullOrWhiteSpace(dto.OfficeId))
                query = query.Where(v => v.OfficeId == Guid.Parse(dto.OfficeId));

            return await PagedList<Section>.CreateAsync(query, dto.page, dto.pageSize);
        }

        public async Task<Section> Create(Section section)
        {
            section.CreatedBy = _userContext.GetUserId();
            _context.Sections.Add(section);

            await _context.SaveChangesAsync();
            return section;
        }

        public async Task<Section> GetById(Guid id)
        {
            var section = await _context.Sections
                .Include(s => s.DepartmentId)
                .FirstOrDefaultAsync(s => s.Id == id);

            if (section is null)
                throw new KeyNotFoundException($"Section record not found");

            return section;
        }

        public async Task<Section> Update(Guid id, Section section)
        {
            var rows = await _context.Database.ExecuteSqlInterpolatedAsync($@"
                UPDATE ""Sections""
                SET ""Name"" = {section.Name}, ""Code"" = {section.Code},""UpdatedBy"" = {_userContext.GetUserId()},
                ""OfficeId"" = {section.OfficeId}, ""DepartmentId"" = {section.DepartmentId}, ""UpdatedAt"" = {DateTime.UtcNow} WHERE ""Id"" = {id};
            ");
            
            if (rows == 0)
                throw new KeyNotFoundException("Section record not found.");

            var updatedSection = await _context.Sections.FindAsync(id);
            return updatedSection ?? throw new KeyNotFoundException("Failed to retrieve updated record");
        }

        public async Task<bool> Delete(Guid id)
        {
            var section = await _context.Sections.FindAsync(id);

            if (section is null)
                throw new KeyNotFoundException("Section record not found");

            section.DeletedAt = DateTime.UtcNow;
            section.UpdatedBy = _userContext.GetUserId();
            section.UpdatedAt = DateTime.UtcNow;
         
            await _context.SaveChangesAsync();
            return true;
        }
        
        public async Task<SectionAndDepartmentDto> GetSectionsAndDepartment(Guid officeId)
        {
            var dataSql = "SELECT * FROM fun_get_sections_departments({0})";
            var results = await _context.Database.SqlQueryRaw<string>(dataSql, officeId)
                .ToListAsync();

            var jsonString = results.FirstOrDefault();
            if (string.IsNullOrWhiteSpace(jsonString)) return new SectionAndDepartmentDto();

            var result = JsonSerializer.Deserialize<SectionAndDepartmentDto>(
                jsonString,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
            );
            return result ?? new SectionAndDepartmentDto();
        }
    }
}
