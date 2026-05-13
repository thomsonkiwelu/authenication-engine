using conservation_backend.Features.LessRangerStations.Interfaces;
using conservation_backend.Shared;
using MapsterMapper;

namespace conservation_backend.Features.LessRangerStations;

public class LessRangerStationService(ILessRangerStationRepository repository, IMapper mapper) : ILessRangerStationService
{
    private readonly ILessRangerStationRepository _repository = repository;
    private readonly IMapper _mapper = mapper;

    public async Task<PagedList<LessRangerStationResponseDto>> GetAllStationsData(LessRangerStationPaginationDto dto)
    {
        var pagedData = await _repository.GetPagedData(dto);
        var startIndex = (pagedData.Page - 1) * pagedData.PageSize + 1;

        var dtoList = _mapper.Map<List<LessRangerStationResponseDto>>(pagedData.Data)
            .Select((item, index) => item with { RowNumber = startIndex + index })
            .ToList();

        return new PagedList<LessRangerStationResponseDto>(
            items: dtoList,
            page: pagedData.Page,
            pageSize: pagedData.PageSize,
            totalCount: pagedData.TotalCount
        );
    }

    public async Task<LessRangerStationDto> CreateStation(LessRangerStationRequest dto)
    {
        var station = _mapper.Map<LessRangerStation>(dto);
        var created = await _repository.Create(station);

        return _mapper.Map<LessRangerStationDto>(created);
    }

    public async Task<LessRangerStationDto> GetStationById(Guid id)
    {
        var station = await _repository.GetById(id);
        return _mapper.Map<LessRangerStationDto>(station);
    }

    public async Task<LessRangerStationDto> UpdateStation(Guid id, LessRangerStationRequest dto)
    {
        var station = _mapper.Map<LessRangerStation>(dto);
        var updated = await _repository.Update(id, station);

        return _mapper.Map<LessRangerStationDto>(updated);
    }

    public async Task<bool> DeleteStation(Guid id)
    {
        return await _repository.Delete(id);
    }
}
