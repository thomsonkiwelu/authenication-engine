using conservation_backend.Features.Regions.Interfaces;
using conservation_backend.Shared;
using MapsterMapper;

namespace conservation_backend.Features.Regions;

public class RegionService(IRegionRepository repository, IMapper mapper): IRegionService
{
    private readonly IRegionRepository _regionRepository = repository;
    private readonly IMapper _mapper = mapper;
    
    public async Task<PagedList<RegionResponseDto>> GetAllRegions(RegionPaginationDto dto)
    {
        var pagedData = await _regionRepository.GetPagedData(dto);

        var startIndex = (pagedData.Page - 1) * pagedData.PageSize + 1;

        var dtoList = _mapper.Map<List<RegionResponseDto>>(pagedData.Data)
            .Select((dto, index) => dto with { RowNumber = startIndex + index }).ToList();

        return new PagedList<RegionResponseDto>(
            items: dtoList,
            page: pagedData.Page,
            pageSize: pagedData.PageSize,
            totalCount: pagedData.TotalCount
        );
    }

    public async Task<RegionDto> CreateRegion(RegionRequest dto)
    {
        var region = _mapper.Map<Region>(dto);
        var result = await _regionRepository.Create(region);
        
        var responseDto = _mapper.Map<RegionDto>(result);
        return responseDto;
    }

    public async Task<RegionDto> GetRegionById(Guid id)
    {
        var region = await _regionRepository.GetById(id);
        
        var result = _mapper.Map<RegionDto>(region);
        
        return result;
    }

    public async Task<RegionDto> UpdateRegion(Guid id, RegionRequest dto)
    {
        var region = _mapper.Map<Region>(dto);
        var updated = await _regionRepository.Update(id, region);

        var responseDto = _mapper.Map<RegionDto>(updated);
        return responseDto;
    }

    public async Task<bool> DeleteRegion(Guid id)
    {
        return await _regionRepository.Delete(id);
    }
}