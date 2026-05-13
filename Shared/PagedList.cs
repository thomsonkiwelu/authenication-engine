
using Microsoft.EntityFrameworkCore;

namespace conservation_backend.Shared
{
    public class PagedList<T>(List<T> items, int page, int pageSize, int totalCount)
    {
        public List<T> Data { get; } = items;
        public int Page { get; } = page;
        public int PageSize { get; } = pageSize;
        public int TotalCount { get; } = totalCount;

        public static async Task<PagedList<T>> CreateAsync(
            IQueryable<T> query, 
            int page, 
            int pageSize
        )
        {
            var totalCount = await query.CountAsync();
            var data = await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
            return new PagedList<T>(data, page, pageSize, totalCount);
        }

        public static async Task<PagedList<T>> CreateFromSqlAsync(
            DbContext context,
            string dataSql,
            int totalCount,
            int page,
            int pageSize,
            string? search = "")
        {

            var data = await context.Database
                .SqlQueryRaw<T>(dataSql, page, pageSize, search ?? "")
                .ToListAsync();

            return new PagedList<T>(data, page, pageSize, totalCount);
        }
    }
}
