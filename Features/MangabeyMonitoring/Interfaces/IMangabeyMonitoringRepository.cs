using conservation_backend.Shared;

namespace conservation_backend.Features.MangabeyMonitoring.Interfaces;

public interface IMangabeyMonitoringRepository
{
    Task<PagedList<MangabeyMonitoring>> GetPagedData(MangabeyMonitoringPaginationDto dto);

    Task<MangabeyMonitoring> Create(MangabeyMonitoring mangabeyMonitoring);

    Task<MangabeyMonitoring> GetById(Guid id);

    Task<MangabeyMonitoring> Update(Guid id, MangabeyMonitoring mangabeyMonitoring);

    Task<bool> Delete(Guid id);
}