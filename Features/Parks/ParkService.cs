using conservation_backend.Features.Parks.Interfaces;
using conservation_backend.Shared;
using MapsterMapper;

namespace conservation_backend.Features.Parks;

public class ParkService(IParkRepository repository, IMapper mapper) : IParkService
{
    private readonly IParkRepository _parkRepository = repository;

    private readonly IMapper _mapper = mapper;
    
    public async Task<PagedList<ParkResponseDto>> GetAllParksData(ParkPaginationDto dto)
    {
        var pagedData = await _parkRepository.GetPagedData(dto);
        var startIndex = (pagedData.Page - 1) * pagedData.PageSize + 1;

        var dtoList = _mapper.Map<List<ParkResponseDto>>(pagedData.Data)
            .Select((dto, index) => dto with { RowNumber = startIndex + index }).ToList();

        return new PagedList<ParkResponseDto>(
            items: dtoList,
            page: pagedData.Page,
            pageSize: pagedData.PageSize,
            totalCount: pagedData.TotalCount
        );
    }

    public async Task<ParkDto> CreatePark(ParkRequest dto)
    {
        var park = _mapper.Map<Park>(dto);
        var createdPark = await _parkRepository.Create(park);
        
        var responseDto = _mapper.Map<ParkDto>(createdPark);
        return responseDto;
    }

    public async Task<ParkDto> GetParkById(Guid id)
    {
        var park = await _parkRepository.GetById(id);

        var result = _mapper.Map<ParkDto>(park);
        return result;
    }

    public async Task<ParkDto> UpdatePark(Guid id, ParkRequest dto)
    {
        var rank = _mapper.Map<Park>(dto);
        var updatedPark = await _parkRepository.Update(id, rank);

        var responseDto = _mapper.Map<ParkDto>(updatedPark);
        return responseDto;
    }

    public async Task<bool> DeletePark(Guid id)
    {
        return await _parkRepository.Delete(id);
    }
}