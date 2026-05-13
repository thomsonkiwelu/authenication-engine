using conservation_backend.Config;
using conservation_backend.Features.Auth;
using conservation_backend.Features.Users.Interfaces;
using conservation_backend.Shared;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace conservation_backend.Features.Users
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

        public async Task<User?> GetUserByUsername(LoginRequest dto)
        {
            var user = await _context.Users
                 .FirstOrDefaultAsync(u => u.Username == dto.Username && u.IsActive);

            if (user == null)
                return null;

            return user;
        }

        public async Task<UserWithAccessControlDto?> GetUserByIdWithAccessControl(Guid Id)
        {
            var dataSql = "SELECT * FROM FUN_GET_USER_BY_ID_WITH_ACCESS_CONTROL({0})";

            var results = await _context.Database.SqlQueryRaw<string>(dataSql, Id)
                .ToListAsync();

            var jsonString = results.FirstOrDefault();

            if (string.IsNullOrEmpty(jsonString) || jsonString == "{}")
                return null;

            var result = JsonSerializer.Deserialize<UserWithAccessControlDto>(
                jsonString,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
            );

            if (result is null)
                return null;

            // Assigned park is derived from the user's office's ParkId
            if (result.Office.ParkId.HasValue)
            {
                var park = await _context.Parks
                    .AsNoTracking()
                    .Where(p => p.Id == result.Office.ParkId.Value)
                    .Select(p => new ParkMinimalDto
                    {
                        Id = p.Id,
                        Name = p.Name,
                        Code = p.Code ?? string.Empty,
                        Zone = p.Zone
                    })
                    .FirstOrDefaultAsync();

                result.AssignedPark = park;
            }

            // Accessible parks are taken from UserParks
            result.AccessibleParks = await _context.UserParks
                .AsNoTracking()
                .Where(up => up.UserId == Id)
                .Join(
                    _context.Parks.AsNoTracking(),
                    up => up.ParkId,
                    p => p.Id,
                    (up, p) => new ParkMinimalDto
                    {
                        Id = p.Id,
                        Name = p.Name,
                        Code = p.Code ?? string.Empty,
                        Zone = p.Zone
                    }
                )
                .ToListAsync();

            return result;
        }

    }
}
