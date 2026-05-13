using conservation_backend.Features.Stations.Interfaces;
using conservation_backend.Shared;
using MapsterMapper;

namespace conservation_backend.Features.Stations;

public class StationService(IStationRepository repository, IMapper mapper) : IStationService
{
    private readonly IStationRepository _stationRepository = repository;
    private readonly IMapper _mapper = mapper;
    
    public async Task<PagedList<StationResponseDto>> GetAllStationsData(StationPaginationDto dto)
    {
        var pagedData = await _stationRepository.GetPagedData(dto);
        var startIndex = (pagedData.Page - 1) * pagedData.PageSize + 1;

        var dtoList = _mapper.Map<List<StationResponseDto>>(pagedData.Data)
            .Select((dto, index) => dto with { RowNumber = startIndex + index }).ToList();

        return new PagedList<StationResponseDto>(
            items: dtoList,
            page: pagedData.Page,
            pageSize: pagedData.PageSize,
            totalCount: pagedData.TotalCount
        );
    }

    public async Task<StationDto> CreateStation(StationRequest dto)
    {
        var station = _mapper.Map<Station>(dto);

        var created = await _stationRepository.Create(station);

        var responseDto = _mapper.Map<StationDto>(created);
        
        return responseDto;
    }

    public async Task<StationDto> GetStationById(Guid id)
    {
        var station = await _stationRepository.GetById(id);

        var result = _mapper.Map<StationDto>(station);

        return result;
    }

    public async Task<StationDto> UpdateStation(Guid id, StationRequest dto)
    {
        var station = _mapper.Map<Station>(dto);

        var updated = await _stationRepository.Update(id, station);

        var responseDto = _mapper.Map<StationDto>(updated);

        return responseDto;
    }

    public async Task<bool> DeleteStation(Guid id)
    {
        return await _stationRepository.Delete(id);
    }
}