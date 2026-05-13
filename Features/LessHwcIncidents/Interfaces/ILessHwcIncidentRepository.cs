namespace conservation_backend.Features.LessHwcIncidents.Interfaces;

public interface ILessHwcIncidentRepository
{
    Task<LessHwcIncident?> GetById(Guid id);

    Task<LessHwcIncident> Create(LessHwcIncident incident);

    Task SaveChanges();
}
