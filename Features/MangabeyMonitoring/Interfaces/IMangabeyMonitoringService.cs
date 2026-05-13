using conservation_backend.Shared;

namespace conservation_backend.Features.MangabeyMonitoring.Interfaces;

public interface IMangabeyMonitoringService
{
    Task<PagedList<MangabeyMonitoringResponseDto>> GetAllMangabeyMonitoringData(MangabeyMonitoringPaginationDto dto);

    Task<MangabeyMonitoringDto> CreateMangabeyMonitoring(MangabeyMonitoringRequestDto dto);

    Task<MangabeyMonitoringDto> GetMangabeyMonitoringById(Guid id);

    Task<MangabeyMonitoringDto> UpdateMangabeyMonitoring(Guid id, MangabeyMonitoringRequestDto dto);

    Task<bool> DeleteMangabeyMonitoring(Guid id);
}