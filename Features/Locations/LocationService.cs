using conservation_backend.Features.Locations.Interfaces;
using conservation_backend.Shared;
using MapsterMapper;

namespace conservation_backend.Features.Locations;

public class LocationService(ILocationRepository repository, IMapper mapper) : ILocationService
{
    private readonly ILocationRepository _locationRepository = repository;
    private readonly IMapper _mapper = mapper;

    public async Task<PagedList<LocationResponseDto>> GetAllLocationsData(LocationPaginationDto dto)
    {
        var pagedData = await _locationRepository.GetPagedData(dto);
        var startIndex = (pagedData.Page - 1) * pagedData.PageSize + 1;

        var dtoList = _mapper.Map<List<LocationResponseDto>>(pagedData.Data)
            .Select((dto, index) => dto with { RowNumber = startIndex + index }).ToList();

        return new PagedList<LocationResponseDto>(
            items: dtoList,
            page: pagedData.Page,
            pageSize: pagedData.PageSize,
            totalCount: pagedData.TotalCount
        );
    }

    public async Task<LocationDto> CreateLocation(LocationRequest dto)
    {
        var location = _mapper.Map<Location>(dto);

        var created = await _locationRepository.Create(location);

        var responseDto = _mapper.Map<LocationDto>(created);
        
        return responseDto;
    }

    public async Task<LocationDto> GetLocationById(Guid id)
    {
        var location = await _locationRepository.GetById(id);

        var result = _mapper.Map<LocationDto>(location);

        return result;
    }

    public async Task<LocationDto> UpdateLocation(Guid id, LocationRequest dto)
    {
        var location = _mapper.Map<Location>(dto);

        var updated = await _locationRepository.Update(id, location);

        var responseDto = _mapper.Map<LocationDto>(updated);

        return responseDto;
    }

    public async Task<bool> DeleteLocation(Guid id)
    {
        return await _locationRepository.Delete(id);
    }
}