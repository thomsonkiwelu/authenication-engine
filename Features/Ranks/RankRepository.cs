using conservation_backend.Config;
using conservation_backend.Features.Ranks.Interfaces;
using conservation_backend.Shared;
using Microsoft.EntityFrameworkCore;

namespace conservation_backend.Features.Ranks
{
    public class RankRepository(AppDBContext context, IUserContext userContext) : IRankRepository
    {
        private readonly AppDBContext _context = context;
        private readonly IUserContext _userContext = userContext;
      
        public async Task<PagedList<Rank>> GetPagedData(PaginationDto dto)
        {
            string[] searchColumns = new string[] { "Name" };
            var query = _context.Ranks.AsNoTracking().AsQueryable();

            //Apply search filter
            query = ApplyFilters<Rank>.ApplySearch(query, dto.q ?? "", searchColumns);

            //Apply sorting filter
            query = ApplyFilters<Rank>.ApplySorting(query, dto.sortBy, dto.sortDesc);

            return await PagedList<Rank>.CreateAsync(query, dto.page, dto.pageSize);
        }

        public async Task<Rank> Create(Rank rank)
        {
            rank.CreatedBy = _userContext.GetUserId();
            _context.Ranks.Add(rank);

            await _context.SaveChangesAsync();
            return rank;
        }

        public async Task<Rank> GetById(Guid id)
        {
            var rank = await _context.Ranks.FindAsync(id);

            if (rank is null)
                throw new KeyNotFoundException($"Ranks record not found");

            return rank;
        }

        public async Task<Rank> Update(Guid id, Rank rank)
        {
            var rows = await _context.Database.ExecuteSqlInterpolatedAsync($@"
                UPDATE ""Ranks""
                SET ""Name"" = {rank.Name}, ""Code"" = {rank.Code},""UpdatedBy"" = {_userContext.GetUserId()},
                ""Category"" = {rank.Category}, ""UpdatedAt"" = {DateTime.UtcNow} WHERE ""Id"" = {id};
            ");
            
            if (rows == 0)
                throw new KeyNotFoundException("Rank record not found.");
            
            var updated = await _context.Ranks.FindAsync(id);
            return updated ?? throw new KeyNotFoundException("Failed to retrieve updated record");
        }

        public async Task<bool> Delete(Guid id)
        {
            var rank = await _context.Ranks.FindAsync(id);

            if (rank is null)
                throw new KeyNotFoundException("Rank record not found");

            rank.DeletedAt = DateTime.UtcNow;
            rank.UpdatedBy = _userContext.GetUserId();
            rank.UpdatedAt = DateTime.UtcNow;
            
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
