using System.Text.Json;
using authentication_engine.Config;
using authentication_engine.Features.Auth;
using authentication_engine.Features.Users.Interfaces;
using authentication_engine.Shared;
using Microsoft.EntityFrameworkCore;

namespace authentication_engine.Features.Users
{
    public class UserRepository(AppDBContext context) : IUserRepository
    {
        private readonly AppDBContext _context = context;

        public async Task<PagedList<User>> GetPagedData(PaginationDto dto)
        {
            string[] searchColumns = new string[] { "Staff.FirstName", "Staff.LastName", "Username", "Email" };
            var query = _context.Users.Include(s => s.Staff)
                .AsNoTracking().AsQueryable();

            //Apply search filter
            query = ApplyFilters<User>.ApplySearch(query, dto.q ?? "", searchColumns);
            
            //Apply sorting filter
            query = ApplyFilters<User>.ApplySorting(query, dto.sortBy, dto.sortDesc);

            return await PagedList<User>.CreateAsync(query, dto.page, dto.pageSize);
        }

        public Task<User> Create(User office)
        {
            throw new NotImplementedException();
        }


        public async Task<UserDetailsDto> GetById(Guid id)
        {
            var dataSql = "SELECT * FROM fun_user_by_id({0})";
            var results = await _context.Database.SqlQueryRaw<string>(dataSql, id)
                .ToListAsync();

            var jsonString = results.FirstOrDefault();
            if (string.IsNullOrWhiteSpace(jsonString)) return new UserDetailsDto();

            var result = JsonSerializer.Deserialize<UserDetailsDto>(
                jsonString,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
            );
            return result ?? new UserDetailsDto();
        }

        public Task<User> Update(Guid id, User office)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Delete(Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task<User?> GetUserByUsername(string username)
        {
            var user = await _context.Users
                 .FirstOrDefaultAsync(u => u.Username == username && u.IsActive);

            if (user == null)
                return null;

            return user;
        }

        public async Task<UserWithAccessControlDto?> GetUserAccessControl(Guid userId, Guid systemApplicationId)
        {
            var dataSql = "SELECT * FROM fun_get_user_access_data({0},{1})";
            var results = await _context.Database.SqlQueryRaw<string>(dataSql, userId, systemApplicationId)
                .ToListAsync();

            var jsonString = results.FirstOrDefault();
            if (string.IsNullOrEmpty(jsonString) || jsonString == "{}") return null;

            var result = JsonSerializer.Deserialize<UserWithAccessControlDto>(
                jsonString,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
            );

            if (result is null) return null;

            return result;
        }

    }
}
