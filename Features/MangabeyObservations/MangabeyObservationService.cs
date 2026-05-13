using conservation_backend.Features.MangabeyObservations.Interfaces;
using conservation_backend.Shared;
using MapsterMapper;

namespace conservation_backend.Features.MangabeyObservations;

public class MangabeyObservationService(IMangabeyObservationRepository repository, IMapper mapper): IMangabeyObservationService
{
    private readonly IMangabeyObservationRepository _mangabeyObservationRepository = repository;
    private readonly IMapper _mapper = mapper;
    
    public async Task<PagedList<MangabeyObservationResponseDto>> GetMangabeyObservations(MangabeyObservationPaginationDto dto)
    {
        var pagedData = await _mangabeyObservationRepository.GetPagedData(dto);
        var startIndex = (pagedData.Page - 1) * pagedData.PageSize + 1;

        var dtoList = _mapper.Map<List<MangabeyObservationResponseDto>>(pagedData.Data)
            .Select((dto, index) => dto with { RowNumber = startIndex + index }).ToList();

        return new PagedList<MangabeyObservationResponseDto>(
            items: dtoList,
            page: pagedData.Page,
            pageSize: pagedData.PageSize,
            totalCount: pagedData.TotalCount
        );
    }

    public async Task<GetMangabeyObservationDto> CreateMangabeyObservation(MangabeyObservationRequestDto dto)
    {
        var mangabeyObservationId = await _mangabeyObservationRepository.Create(dto);
        
        if (string.IsNullOrWhiteSpace(mangabeyObservationId))
            throw new ArgumentNullException("Failure to create mangabey observation data");

        return await _mangabeyObservationRepository.GetById(Guid.Parse(mangabeyObservationId));
    }

    public async Task<GetMangabeyObservationDto> GetMangabeyObservationById(Guid id)
    {
        return await _mangabeyObservationRepository.GetById(id);
    }

    public async Task<GetMangabeyObservationDto> UpdateMangabeyObservation(Guid id, MangabeyObservationRequestDto dto)
    {
        var mangabeyObservationId = await _mangabeyObservationRepository.Update(id, dto);
        
        if (string.IsNullOrWhiteSpace(mangabeyObservationId))
            throw new ArgumentNullException("Failure to update mangabey observation data");
        
        return await _mangabeyObservationRepository.GetById(Guid.Parse(mangabeyObservationId));
    }

    public async Task<bool> DeleteMangabeyObservation(Guid id)
    {
        return await _mangabeyObservationRepository.Delete(id);
    }
}