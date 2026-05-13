using authentication_engine.Shared;

namespace authentication_engine.Features.LessOperationalZones.Interfaces;

public interface ILessOperationalZoneRepository
{
    Task<PagedList<LessOperationalZone>> GetPagedData(LessOperationalZonePaginationDto dto);

    Task<LessOperationalZone> Create(LessOperationalZone zone);

    Task<LessOperationalZone> GetById(Guid id);

    Task<LessOperationalZone> Update(Guid id, LessOperationalZone zone);

    Task<bool> Delete(Guid id);
}
