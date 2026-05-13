using conservation_backend.Features.WaterBodies.Interfaces;
using conservation_backend.Shared;
using MapsterMapper;

namespace conservation_backend.Features.WaterBodies;

public class WaterBodyService(IWaterBodyRepository repository, IMapper mapper): IWaterBodyService
{
    private readonly IWaterBodyRepository _waterBodyRepository = repository;
    private readonly IMapper _mapper = mapper;
    
    public async Task<PagedList<WaterBodyResponseDto>> GetAllWaterBodiesData(WaterBodyPaginationDto dto)
    {
        var pagedData = await _waterBodyRepository.GetPagedData(dto);
        var startIndex = (pagedData.Page - 1) * pagedData.PageSize + 1;

        var dtoList = _mapper.Map<List<WaterBodyResponseDto>>(pagedData.Data)
            .Select((dto, index) => dto with { RowNumber = startIndex + index }).ToList();

        return new PagedList<WaterBodyResponseDto>(
            items: dtoList,
            page: pagedData.Page,
            pageSize: pagedData.PageSize,
            totalCount: pagedData.TotalCount
        );
    }

    public async Task<WaterBodyDto> CreateWaterBody(WaterBodyRequest dto)
    {
        var waterBody = _mapper.Map<WaterBody>(dto);

        var created = await _waterBodyRepository.Create(waterBody);

        var responseDto = _mapper.Map<WaterBodyDto>(created);
        
        return responseDto;
    }

    public async Task<WaterBodyDto> GetWaterBodyById(Guid id)
    {
        var waterBody = await _waterBodyRepository.GetById(id);

        var result = _mapper.Map<WaterBodyDto>(waterBody);

        return result;
    }

    public  async Task<WaterBodyDto> UpdateWaterBody(Guid id, WaterBodyRequest dto)
    {
        var waterBody = _mapper.Map<WaterBody>(dto);

        var updated = await _waterBodyRepository.Update(id, waterBody);

        var responseDto = _mapper.Map<WaterBodyDto>(updated);
        return responseDto;
    }

    public async Task<bool> DeleteWaterBody(Guid id)
    {
        return await _waterBodyRepository.Delete(id);
    }
}