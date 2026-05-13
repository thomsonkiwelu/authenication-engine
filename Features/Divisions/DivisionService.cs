using conservation_backend.Features.Divisions.Interfaces;
using conservation_backend.Shared;
using MapsterMapper;

namespace conservation_backend.Features.Divisions;

public class DivisionService(IDivisionRepository repository, IMapper mapper): IDivisionService
{
    private readonly IDivisionRepository _divisionRepository = repository;
    private readonly IMapper _mapper = mapper;
    
    public async Task<PagedList<DivisionResponseDto>> GetAllDivisions(DivisionPaginationDto dto)
    {
        var pagedData = await _divisionRepository.GetPagedData(dto);

        var startIndex = (pagedData.Page - 1) * pagedData.PageSize + 1;

        var dtoList = _mapper.Map<List<DivisionResponseDto>>(pagedData.Data)
            .Select((dto, index) => dto with { RowNumber = startIndex + index }).ToList();

        return new PagedList<DivisionResponseDto>(
            items: dtoList,
            page: pagedData.Page,
            pageSize: pagedData.PageSize,
            totalCount: pagedData.TotalCount
        );
    }

    public async Task<DivisionDto> CreateDivision(DivisionRequest dto)
    {
        var division = _mapper.Map<Division>(dto);
        var result = await _divisionRepository.Create(division);
        
        var responseDto = _mapper.Map<DivisionDto>(result);
        return responseDto;
    }

    public async Task<DivisionDto> GetDivisionById(Guid id)
    {
        var division = await _divisionRepository.GetById(id);
        
        var result = _mapper.Map<DivisionDto>(division);
        return result;
    }

    public async Task<DivisionDto> UpdateDivision(Guid id, DivisionRequest dto)
    {
        var division = _mapper.Map<Division>(dto);
        var updated = await _divisionRepository.Update(id, division);

        var responseDto = _mapper.Map<DivisionDto>(updated);
        return responseDto;
    }

    public async Task<bool> DeleteDivision(Guid id)
    {
        return await _divisionRepository.Delete(id);
    }
}