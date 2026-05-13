using authentication_engine.Config;
using authentication_engine.Features.Offices.Interface;
using authentication_engine.Shared;
using Microsoft.EntityFrameworkCore;

namespace authentication_engine.Features.Offices
{
    public class OfficeRepository(AppDBContext context, IUserContext userContext): IOfficeRepository
    {
        private readonly AppDBContext _context = context;
        private readonly IUserContext _userContext = userContext;

        public async Task<PagedList<Office>> GetPagedData(OfficePaginationDto dto)
        {
            string[] searchColumns = new string[] { "Name" };
            var query = _context.Offices.AsNoTracking().AsQueryable();

            //Apply search filter
            query = ApplyFilters<Office>.ApplySearch(query, dto.q ?? "", searchColumns);

            //Apply sorting filter
            query = ApplyFilters<Office>.ApplySorting(query, dto.sortBy, dto.sortDesc);

            if (!string.IsNullOrWhiteSpace(dto.StructureId))
                query = query.Where(v => v.StructureId == Guid.Parse(dto.StructureId));

            return await PagedList<Office>.CreateAsync(query, dto.page, dto.pageSize);
        }

        public async Task<Office> Create(Office office)
        {
            office.CreatedBy = _userContext.GetUserId();
            _context.Offices.Add(office);

            await _context.SaveChangesAsync();
            return office;
        }

        public async Task<Office> GetById(Guid id)
        {
            var office = await _context.Offices
                .Include(s => s.Structure)
                .FirstOrDefaultAsync(s => s.Id == id);

            if (office is null)
                throw new KeyNotFoundException($"Office record not found");

            return office;
        }

        public async Task<Office> Update(Guid id, Office office)
        {
            var rows = await _context.Database.ExecuteSqlInterpolatedAsync($@"
                UPDATE ""Offices""
                SET 
                    ""Name"" = {office.Name}, 
                    ""Code"" = CASE 
                        WHEN ""ParentOffice"" = 1 THEN ""Code"" 
                        ELSE {office.Code}
                    END,
                    ""HeadOfOffice"" = {office.HeadOfOffice},
                    ""UpdatedBy"" = {_userContext.GetUserId()},
                    ""UpdatedAt"" = {DateTime.UtcNow}
                WHERE ""Id"" = {id};
            ");
            
            if (rows == 0)
                throw new KeyNotFoundException("Office record not found.");

            var updatedOffice = await _context.Offices.FindAsync(id);
            return updatedOffice ?? throw new KeyNotFoundException("Failed to retrieve updated record");
        }

        public async Task<bool> Delete(Guid id)
        {
            var office = await _context.Offices.FindAsync(id);

            if (office is null)
                throw new KeyNotFoundException("Office record not found");

            office.DeletedAt = DateTime.UtcNow;
            //INFO: update details
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
