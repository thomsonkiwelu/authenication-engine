using conservation_backend.Features.Wards.Interfaces;
using conservation_backend.Shared;
using MapsterMapper;

namespace conservation_backend.Features.Wards;

public class WardService(IWardRepository repository, IMapper mapper): IWardService
{
    private readonly IWardRepository _wardRepository = repository;
    private readonly IMapper _mapper = mapper;
    
    public async Task<PagedList<WardResponseDto>> GetAllWards(WardPaginationDto dto)
    {
        var pagedData = await _wardRepository.GetPagedData(dto);

        var startIndex = (pagedData.Page - 1) * pagedData.PageSize + 1;

        var dtoList = _mapper.Map<List<WardResponseDto>>(pagedData.Data)
            .Select((dto, index) => dto with { RowNumber = startIndex + index }).ToList();

        return new PagedList<WardResponseDto>(
            items: dtoList,
            page: pagedData.Page,
            pageSize: pagedData.PageSize,
            totalCount: pagedData.TotalCount
        );
    }

    public async Task<WardDto> CreateWard(WardRequest dto)
    {
        var ward = _mapper.Map<Ward>(dto);
        var result = await _wardRepository.Create(ward);
        
        var responseDto = _mapper.Map<WardDto>(result);
        return responseDto;
    }

    public async Task<WardDto> GetWardById(Guid id)
    {
        var ward = await _wardRepository.GetById(id);
        
        var result = _mapper.Map<WardDto>(ward);
        return result;
    }

    public async Task<WardDto> UpdateWard(Guid id, WardRequest dto)
    {
        var ward = _mapper.Map<Ward>(dto);
        var updated = await _wardRepository.Update(id, ward);

        var responseDto = _mapper.Map<WardDto>(updated);
        return responseDto;
    }

    public async Task<bool> DeleteWard(Guid id)
    {
        return await _wardRepository.Delete(id);
    }
}