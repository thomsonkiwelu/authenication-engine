using conservation_backend.Config;
using conservation_backend.Features.Species.Interfaces;
using conservation_backend.Shared;
using Microsoft.EntityFrameworkCore;

namespace conservation_backend.Features.Species
{
    public class SpeciesRepository(AppDBContext context, IUserContext userContext): ISpeciesRepository
    {
        private readonly AppDBContext _context = context;
        private readonly IUserContext _userContext = userContext;

        public async Task<PagedList<Species>> GetPagedData(SpeciesPaginationDto dto)
        {
            string[] searchColumns = new string[] { "CommonName", "ScientificName" };
            var query = _context.Species.Include(u => u.Creator).AsNoTracking().AsQueryable();

            //Apply search filter
            query = ApplyFilters<Species>.ApplySearch(query, dto.q ?? "", searchColumns);

            //Apply sorting filter
            query = ApplyFilters<Species>.ApplySorting(query, dto.sortBy, dto.sortDesc);

            if (dto.Type.HasValue)
                query = query.Where(v => v.Type == dto.Type);

            return await PagedList<Species>.CreateAsync(query, dto.page, dto.pageSize);
        }

        public async Task<Species> Create(Species species)
        {
            species.CreatedBy = _userContext.GetUserId();
            _context.Species.Add(species);

            await _context.SaveChangesAsync();
            return species;
        }

        public async Task<Species> GetById(Guid id)
        {
            var species = await _context.Species.FindAsync(id);

            if (species is null)
                throw new KeyNotFoundException($"Species record not found");

            return species;
        }

        public async Task<Species> Update(Guid id, Species species)
        {
            var rows = await _context.Database.ExecuteSqlInterpolatedAsync($@"
                UPDATE ""Species""
                SET ""CommonName"" = {species.CommonName},""ScientificName"" = {species.ScientificName},""Type"" = {species.Type},
                    ""UpdatedBy"" = {_userContext.GetUserId()},
                ""UpdatedAt"" = {DateTime.UtcNow} WHERE ""Id"" = {id};
            ");
            
            if (rows == 0)
                throw new KeyNotFoundException("Species record not found.");
            
            var updated = await _context.Species.FindAsync(id);
            return updated ?? throw new KeyNotFoundException("Failed to retrieve updated record");
        }

        public async Task<bool> Delete(Guid id)
        {
            var species = await _context.Species.FindAsync(id);

            if (species is null)
                throw new KeyNotFoundException("Species record not found");

            species.DeletedAt = DateTime.UtcNow;
            species.UpdatedBy = _userContext.GetUserId();
            species.UpdatedAt = DateTime.UtcNow;
            
            await _context.SaveChangesAsync();
            return true;
        }

    }
}
