using conservation_backend.Features.LessOperationalZones.Interfaces;
using conservation_backend.Shared;
using MapsterMapper;

namespace conservation_backend.Features.LessOperationalZones;

public class LessOperationalZoneService(ILessOperationalZoneRepository repository, IMapper mapper) : ILessOperationalZoneService
{
    private readonly ILessOperationalZoneRepository _repository = repository;
    private readonly IMapper _mapper = mapper;

    public async Task<PagedList<LessOperationalZoneResponseDto>> GetAllZonesData(LessOperationalZonePaginationDto dto)
    {
        var pagedData = await _repository.GetPagedData(dto);
        var startIndex = (pagedData.Page - 1) * pagedData.PageSize + 1;

        var dtoList = _mapper.Map<List<LessOperationalZoneResponseDto>>(pagedData.Data)
            .Select((item, index) => item with { RowNumber = startIndex + index })
            .ToList();

        return new PagedList<LessOperationalZoneResponseDto>(
            items: dtoList,
            page: pagedData.Page,
            pageSize: pagedData.PageSize,
            totalCount: pagedData.TotalCount
        );
    }

    public async Task<LessOperationalZoneDto> CreateZone(LessOperationalZoneRequest dto)
    {
        var zone = _mapper.Map<LessOperationalZone>(dto);
        var created = await _repository.Create(zone);

        return _mapper.Map<LessOperationalZoneDto>(created);
    }

    public async Task<LessOperationalZoneDto> GetZoneById(Guid id)
    {
        var zone = await _repository.GetById(id);
        return _mapper.Map<LessOperationalZoneDto>(zone);
    }

    public async Task<LessOperationalZoneDto> UpdateZone(Guid id, LessOperationalZoneRequest dto)
    {
        var zone = _mapper.Map<LessOperationalZone>(dto);
        var updated = await _repository.Update(id, zone);

        return _mapper.Map<LessOperationalZoneDto>(updated);
    }

    public async Task<bool> DeleteZone(Guid id)
    {
        return await _repository.Delete(id);
    }
}
