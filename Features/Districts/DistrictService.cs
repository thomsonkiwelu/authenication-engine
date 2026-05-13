using conservation_backend.Features.Districts.Interfaces;
using conservation_backend.Shared;
using MapsterMapper;

namespace conservation_backend.Features.Districts;

public class DistrictService(IDistrictRepository repository, IMapper mapper): IDistrictService
{
    private readonly IDistrictRepository _districtRepository = repository;
    private readonly IMapper _mapper = mapper;
    
    public async Task<PagedList<DistrictResponseDto>> GetAllDistricts(DistrictPaginationDto dto)
    {
        var pagedData = await _districtRepository.GetPagedData(dto);

        var startIndex = (pagedData.Page - 1) * pagedData.PageSize + 1;

        var dtoList = _mapper.Map<List<DistrictResponseDto>>(pagedData.Data)
            .Select((dto, index) => dto with { RowNumber = startIndex + index }).ToList();

        return new PagedList<DistrictResponseDto>(
            items: dtoList,
            page: pagedData.Page,
            pageSize: pagedData.PageSize,
            totalCount: pagedData.TotalCount
        );
    }

    public async Task<DistrictDto> CreateDistrict(DistrictRequest dto)
    {
        var region = _mapper.Map<District>(dto);
        var result = await _districtRepository.Create(region);
        
        var responseDto = _mapper.Map<DistrictDto>(result);
        return responseDto;
    }

    public async Task<DistrictDto> GetDistrictById(Guid id)
    {
        var region = await _districtRepository.GetById(id);
        
        var result = _mapper.Map<DistrictDto>(region);
        
        return result;
    }

    public async Task<DistrictDto> UpdateDistrict(Guid id, DistrictRequest dto)
    {
        var district = _mapper.Map<District>(dto);
        
        var updated = await _districtRepository.Update(id, district);

        var responseDto = _mapper.Map<DistrictDto>(updated);
        return responseDto;
    }

    public async Task<bool> DeleteDistrict(Guid id)
    {
        return await _districtRepository.Delete(id);
    }
}