using System.Text.Json;
using conservation_backend.Config;
using conservation_backend.Features.Weathers.Interfaces;
using conservation_backend.Shared;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using NpgsqlTypes;

namespace conservation_backend.Features.Weathers;

public class WeatherRepository(AppDBContext context, IUserContext userContext): IWeatherRepository
{
    private readonly AppDBContext _context = context;
    private readonly IUserContext _userContext = userContext;
    
    public async Task<PagedList<Weather>> GetPagedData(WeatherPaginationDto dto)
    {
        string[] searchColumns = new string[] { "Station.Name" };
        var query = _context.Weather
            .Include(p => p.Park)
            .Include(s => s.Station)
            .Include(u => u.Creator)
            .AsNoTracking().AsQueryable();
        
        //Apply search filter
        query = ApplyFilters<Weather>.ApplySearch(query, dto.q ?? "", searchColumns);
        
        //Apply sorting filter
        query = ApplyFilters<Weather>.ApplySorting(query, dto.sortBy, dto.sortDesc);
        
        //Apply Park filter
        var parkIds = _userContext.GetAuthorizedParkIds(_context);
        if (parkIds.Any())
            query = query.Where(v => v.ParkId.HasValue && parkIds.Contains(v.ParkId.Value));
        
        if (!string.IsNullOrWhiteSpace(dto.ParkId))
            query = query.Where(v => v.ParkId == Guid.Parse(dto.ParkId));
        
        if (!string.IsNullOrWhiteSpace(dto.Method))
            query = query.Where(v => v.CollectionMethod == dto.Method);

        return await PagedList<Weather>.CreateAsync(query, dto.page, dto.pageSize);
    }

    public async Task<Weather> Create(Weather weather)
    {
        weather.CreatedBy = _userContext.GetUserId();
        
        var station = await _context.Stations.FindAsync(weather.StationId);
        weather.ParkId = station!.ParkId;
        
        _context.Weather.Add(weather);
        await _context.SaveChangesAsync();
        return weather;
    }

    public async Task<WeatherDto> GetById(Guid id)
    {
        var dataSql = "SELECT fn_weather_by_id({0})";
        var results = await _context.Database.SqlQueryRaw<string>(dataSql, id)
            .ToListAsync();

        var jsonString = results.FirstOrDefault();
        
        if (string.IsNullOrWhiteSpace(jsonString))
            throw new ArgumentNullException("Failure to get weather data");

        var result = JsonSerializer.Deserialize<WeatherDto>(
            jsonString,
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
        );
        return result ?? new WeatherDto();
    }

    public async Task<string> Update(Guid id, WeatherRequest dto)
    {
        dto.UpdatedBy = _userContext.GetUserId();
        var dtoToJson = JsonSerializer.Serialize(dto);
            
        await using var connection = new NpgsqlConnection(_context.Database.GetConnectionString());
        await connection.OpenAsync();
        await using var command = new NpgsqlCommand(
            "SELECT fn_update_weather(@data::jsonb, @weatherId)",
            connection
        );
        command.Parameters.AddWithValue("data", NpgsqlDbType.Jsonb, dtoToJson);
        command.Parameters.AddWithValue("weatherId", NpgsqlDbType.Varchar, id.ToString());
        var result = await command.ExecuteScalarAsync();
        
        return result?.ToString() ?? string.Empty;
    }

    public async Task<bool> Delete(Guid id)
    {
        var weather = await _context.Weather.FindAsync(id);
        if (weather is null)
            throw new KeyNotFoundException("Weather record not found");

        weather.DeletedAt = DateTime.UtcNow;
        weather.UpdatedBy = _userContext.GetUserId();
        weather.UpdatedAt = DateTime.UtcNow;
         
        await _context.SaveChangesAsync();
        return true;
    }
    
}