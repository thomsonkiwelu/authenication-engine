using conservation_backend.Shared;

namespace conservation_backend.Features.AerialCensuses.Interfaces;

public interface IAerialCensusRepository
{
    Task<PagedList<AerialCensusResponseDto>> GetPagedData(AerialCensusPaginationDto dto);

    Task<string> Create(AerialCensusRequestDto dto);

    Task<GetAerialCensusDto> GetById(Guid id);

    Task<string> Update(Guid id, AerialCensusRequestDto dto);

    Task<bool> Delete(Guid id);
}