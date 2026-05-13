using conservation_backend.Features.MangabeyMonitoring.Interfaces;
using conservation_backend.Shared;
using MapsterMapper;

namespace conservation_backend.Features.MangabeyMonitoring;

public class MangabeyMonitoringService(IMangabeyMonitoringRepository repository, IMapper mapper) : IMangabeyMonitoringService
{
    private readonly IMangabeyMonitoringRepository _mangabeyMonitoringRepository = repository;
    private readonly IMapper _mapper = mapper;
    
    public async Task<PagedList<MangabeyMonitoringResponseDto>> GetAllMangabeyMonitoringData(MangabeyMonitoringPaginationDto dto)
    {
        var pagedData = await _mangabeyMonitoringRepository.GetPagedData(dto);
        var startIndex = (pagedData.Page - 1) * pagedData.PageSize + 1;

        var dtoList = _mapper.Map<List<MangabeyMonitoringResponseDto>>(pagedData.Data)
            .Select((dto, index) => dto with { RowNumber = startIndex + index }).ToList();

        return new PagedList<MangabeyMonitoringResponseDto>(
            items: dtoList,
            page: pagedData.Page,
            pageSize: pagedData.PageSize,
            totalCount: pagedData.TotalCount
        );
    }

    public async Task<MangabeyMonitoringDto> CreateMangabeyMonitoring(MangabeyMonitoringRequestDto dto)
    {
        var mangabeyMonitoring = _mapper.Map<MangabeyMonitoring>(dto);

        var created = await _mangabeyMonitoringRepository.Create(mangabeyMonitoring);

        var responseDto = _mapper.Map<MangabeyMonitoringDto>(created);
        
        return responseDto;
    }

    public async Task<MangabeyMonitoringDto> GetMangabeyMonitoringById(Guid id)
    {
        var mangabeyMonitoring = await _mangabeyMonitoringRepository.GetById(id);

        var result = _mapper.Map<MangabeyMonitoringDto>(mangabeyMonitoring);

        return result;
    }

    public async Task<MangabeyMonitoringDto> UpdateMangabeyMonitoring(Guid id, MangabeyMonitoringRequestDto dto)
    {
        var mangabeyMonitoring = _mapper.Map<MangabeyMonitoring>(dto);

        var updated = await _mangabeyMonitoringRepository.Update(id, mangabeyMonitoring);

        var responseDto = _mapper.Map<MangabeyMonitoringDto>(updated);

        return responseDto;
    }

    public async Task<bool> DeleteMangabeyMonitoring(Guid id)
    {
        return await _mangabeyMonitoringRepository.Delete(id);
    }
}