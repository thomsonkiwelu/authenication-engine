using conservation_backend.Config;
using conservation_backend.Features.LessHwcIncidents.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace conservation_backend.Features.LessHwcIncidents;

public class LessHwcIncidentRepository(AppDBContext context) : ILessHwcIncidentRepository
{
    private readonly AppDBContext _context = context;

    public Task<LessHwcIncident?> GetById(Guid id)
    {
        return _context.LessHwcIncidents
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<LessHwcIncident> Create(LessHwcIncident incident)
    {
        _context.LessHwcIncidents.Add(incident);
        await _context.SaveChangesAsync();
        return incident;
    }

    public Task SaveChanges()
    {
        return _context.SaveChangesAsync();
    }
}
