using conservation_backend.Features.Tribes.Interfaces;
using conservation_backend.Shared;
using MapsterMapper;

namespace conservation_backend.Features.Tribes;

public class TribeService(ITribeRepository repository, IMapper mapper): ITribeService
{
    private readonly ITribeRepository _tribeRepository = repository;
    private readonly IMapper _mapper = mapper;
    
    public async Task<PagedList<TribeResponseDto>> GetAllTribesData(PaginationDto dto)
    {
        var pagedData = await _tribeRepository.GetPagedData(dto);

        var startIndex = (pagedData.Page - 1) * pagedData.PageSize + 1;

        var dtoList = _mapper.Map<List<TribeResponseDto>>(pagedData.Data)
            .Select((dto, index) => dto with { RowNumber = startIndex + index }).ToList();

        return new PagedList<TribeResponseDto>(
            items: dtoList,
            page: pagedData.Page,
            pageSize: pagedData.PageSize,
            totalCount: pagedData.TotalCount
        );
    }

    public async Task<TribeDto> CreateTribe(TribeRequestDto dto)
    {
        var tribe = _mapper.Map<Tribe>(dto);

        var createdTribe = await _tribeRepository.Create(tribe);

        var responseDto = _mapper.Map<TribeDto>(createdTribe);

        return responseDto;
    }

    public async Task<TribeDto> GetTribeById(Guid id)
    {
        var tribe = await _tribeRepository.GetById(id);

        var result = _mapper.Map<TribeDto>(tribe);

        return result;
    }

    public async Task<TribeDto> UpdateTribe(Guid id, TribeRequestDto dto)
    {
        var tribe = _mapper.Map<Tribe>(dto);

        var updated = await _tribeRepository.Update(id, tribe);

        var responseDto = _mapper.Map<TribeDto>(updated);

        return responseDto;
    }

    public async Task<bool> DeleteTribe(Guid id)
    {
        return await _tribeRepository.Delete(id);
    }
}