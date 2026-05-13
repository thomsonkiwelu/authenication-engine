using conservation_backend.Features.AerialCensuses.Interfaces;
using conservation_backend.Shared;
using MapsterMapper;

namespace conservation_backend.Features.AerialCensuses;

public class AerialCensusService(IAerialCensusRepository repository): IAerialCensusService
{
    private readonly IAerialCensusRepository _aerialCensusRepository = repository;

    public async Task<PagedList<AerialCensusResponseDto>> GetPagedAerialCensuses(AerialCensusPaginationDto dto)
    {
        var pagedData = await _aerialCensusRepository.GetPagedData(dto);
        
        return new PagedList<AerialCensusResponseDto>(
            items: pagedData.Data,
            page: pagedData.Page,
            pageSize: pagedData.PageSize,
            totalCount: pagedData.TotalCount
        );
    }

    public async Task<GetAerialCensusDto> CreateAerialCensus(AerialCensusRequestDto dto)
    {
        var aerialCensusId = await _aerialCensusRepository.Create(dto);
        
        if (string.IsNullOrWhiteSpace(aerialCensusId))
            throw new ArgumentNullException("Failure to create aerial census data");
        
        return await _aerialCensusRepository.GetById(Guid.Parse(aerialCensusId));
    }

    public async Task<GetAerialCensusDto> GetAerialCensusById(Guid id)
    {
        return await _aerialCensusRepository.GetById(id);
    }

    public async Task<GetAerialCensusDto> UpdateAerialCensus(Guid id, AerialCensusRequestDto dto)
    {
        var aerialCensusId = await _aerialCensusRepository.Update(id, dto);
        
        if (string.IsNullOrWhiteSpace(aerialCensusId))
            throw new ArgumentNullException("Failure to update aerial census data");
        
        return await _aerialCensusRepository.GetById(Guid.Parse(aerialCensusId));
    }

    public async Task<bool> DeleteAerialCensus(Guid id)
    {
        return await _aerialCensusRepository.Delete(id);
    }
}