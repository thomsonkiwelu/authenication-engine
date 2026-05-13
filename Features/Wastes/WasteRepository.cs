using conservation_backend.Config;
using conservation_backend.Features.Wastes.Interfaces;
using conservation_backend.Shared;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using Npgsql;
using NpgsqlTypes;

namespace conservation_backend.Features.Wastes
{
    public class WasteRepository(AppDBContext context, IUserContext userContext) : IWasteRepository
    {
        private readonly AppDBContext _context = context;
        private readonly IUserContext _userContext = userContext;

        public async Task<PagedList<WasteResponseDto>> GetPagedData(WastePaginationDto dto)
        {
            var connectionString = _context.Database.GetConnectionString();
            var parkIds = _userContext.GetAuthorizedParkIds(_context);
            await using var connection = new NpgsqlConnection(connectionString);
            await connection.OpenAsync();
            await using var command = new NpgsqlCommand(
                "SELECT fn_wastes(@page, @pageSize, @search, @category, @parkId, @parkIds)",
                connection
            );
    
            // Add parameters with proper null handling
            command.Parameters.AddWithValue("page", dto.page);
            command.Parameters.AddWithValue("pageSize", dto.pageSize);
            command.Parameters.AddWithValue("search", NpgsqlDbType.Varchar,
                string.IsNullOrWhiteSpace(dto.q) ? (object)DBNull.Value : dto.q);
            command.Parameters.AddWithValue("category", NpgsqlDbType.Varchar, 
                string.IsNullOrWhiteSpace(dto.Category) ? (object)DBNull.Value : dto.Category);
            command.Parameters.AddWithValue("parkId", NpgsqlDbType.Varchar,
                string.IsNullOrWhiteSpace(dto.ParkId) ? (object)DBNull.Value : dto.ParkId);
            // Add park IDs parameter
            command.Parameters.AddWithValue("parkIds", NpgsqlDbType.Array | NpgsqlDbType.Uuid,
                parkIds.Any() ? parkIds.ToArray() : (object)DBNull.Value);
    
            var jsonResult = await command.ExecuteScalarAsync() as string;
            if (string.IsNullOrWhiteSpace(jsonResult)) throw new ArgumentNullException("Failure to get waste data");
        
            var apiResponse = JsonSerializer.Deserialize<WasteSqlResponseDto>(
                jsonResult,
                new JsonSerializerOptions {
                    PropertyNameCaseInsensitive = true,
                }
            );
    
            return new PagedList<WasteResponseDto>(
                apiResponse!.Data,
                dto.page,
                dto.pageSize,
                apiResponse.Meta.TotalItems
            );
        }

        public async Task<string> Create(WasteRequestDto dto)
        {
            dto.CreatedBy = _userContext.GetUserId();
            var dtoToJson = JsonSerializer.Serialize(dto);
            
            await using var connection = new NpgsqlConnection(_context.Database.GetConnectionString());
            await connection.OpenAsync();
            await using var command = new NpgsqlCommand(
                "SELECT fn_create_waste(@data::jsonb)",
                connection
            );
            command.Parameters.AddWithValue("data", NpgsqlDbType.Jsonb, dtoToJson);
            var result = await command.ExecuteScalarAsync();
        
            return result?.ToString() ?? string.Empty;
        }

        public async Task<GetWasteDto> GetById(Guid id)
        {
            var dataSql = "SELECT fn_waste_by_id({0})";
            var results = await _context.Database.SqlQueryRaw<string>(dataSql, id)
                .ToListAsync();

            var jsonString = results.FirstOrDefault();
        
            if (string.IsNullOrWhiteSpace(jsonString))
                throw new ArgumentNullException("Failure to get waste data");

            var result = JsonSerializer.Deserialize<GetWasteDto>(
                jsonString,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
            );
            return result ?? new GetWasteDto();
        }

        public async Task<string> Update(Guid id, WasteRequestDto dto)
        {
            dto.UpdatedBy = _userContext.GetUserId();
            var dtoToJson = JsonSerializer.Serialize(dto);
            
            await using var connection = new NpgsqlConnection(_context.Database.GetConnectionString());
            await connection.OpenAsync();
            await using var command = new NpgsqlCommand(
                "SELECT fn_update_waste(@data::jsonb, @wasteId)",
                connection
            );
            command.Parameters.AddWithValue("data", NpgsqlDbType.Jsonb, dtoToJson);
            command.Parameters.AddWithValue("wasteId", NpgsqlDbType.Varchar, id.ToString());
            var result = await command.ExecuteScalarAsync();
        
            return result?.ToString() ?? string.Empty;
        }

        public async Task<bool> Delete(Guid id)
        {
            var waste = await _context.Wastes.FindAsync(id);
            if (waste is null)
                throw new KeyNotFoundException("Waste record not found");

            waste.DeletedAt = DateTime.UtcNow;
            waste.UpdatedBy = _userContext.GetUserId();
            waste.UpdatedAt = DateTime.UtcNow;
         
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
