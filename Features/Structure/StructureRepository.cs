using authentication_engine.Config;
using authentication_engine.Features.Structure.Interfaces;
using authentication_engine.Shared;
using Microsoft.EntityFrameworkCore;

namespace authentication_engine.Features.Structure
{
    public class StructureRepository(AppDBContext context, IUserContext userContext): IStructureRepository
    {
        private readonly AppDBContext _context = context;
        private readonly IUserContext _userContext = userContext;

        public async Task<PagedList<StructureEntity>> GetPagedData(PaginationDto dto)
        {
            string[] searchColumns = new string[] { "Name" };
            var query = _context.Structures.AsNoTracking().AsQueryable();

            //Apply search filter
            query = ApplyFilters<StructureEntity>.ApplySearch(query, dto.q ?? "", searchColumns);

            //Apply sorting filter
            query = ApplyFilters<StructureEntity>.ApplySorting(query, dto.sortBy, dto.sortDesc);

            return await PagedList<StructureEntity>.CreateAsync(query, dto.page, dto.pageSize);
        }

        public async Task<StructureEntity> Create(StructureEntity structure)
        {
            structure.CreatedBy = _userContext.GetUserId();
            _context.Structures.Add(structure);

            await _context.SaveChangesAsync();
            return structure;
        }

        public async Task<StructureEntity> GetById(Guid id)
        {
            var structureWithOffices = await context.Structures
                .Include(s => s.Offices)
                .FirstOrDefaultAsync(s => s.Id == id);

            if (structureWithOffices is null)
                throw new KeyNotFoundException($"Organization structure record not found");

            return structureWithOffices;
        }

        public async Task<StructureEntity> Update(Guid id, StructureEntity structure)
        {
            var rows = await _context.Database.ExecuteSqlInterpolatedAsync($@"
                UPDATE ""Structures""
                SET ""Name"" = {structure.Name},""UpdatedBy"" = {_userContext.GetUserId()},""Level"" = {structure.Level},
                ""UpdatedAt"" = {DateTime.UtcNow} WHERE ""Id"" = {id};
            ");
            
            if (rows == 0)
                throw new KeyNotFoundException("Structure record not found.");
            
            var updatedStructure = await _context.Structures.FindAsync(id);
            return updatedStructure ?? throw new KeyNotFoundException("Failed to retrieve updated record");
        }

        public async Task<bool> Delete(Guid id)
        {
            var structure = await _context.Structures.FindAsync(id);
            
            if (structure is null)
                throw new KeyNotFoundException("Organization structure record not found");

            structure.DeletedAt = DateTime.UtcNow;
            structure.UpdatedBy = _userContext.GetUserId();
            structure.UpdatedAt = DateTime.UtcNow;
         
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
