using conservation_backend.Shared;

namespace conservation_backend.Features.AerialCensuses.Interfaces;

public interface IAerialCensusService
{
    Task<PagedList<AerialCensusResponseDto>> GetPagedAerialCensuses(AerialCensusPaginationDto dto);
    
    Task<GetAerialCensusDto> CreateAerialCensus(AerialCensusRequestDto dto);
    
    Task<GetAerialCensusDto> GetAerialCensusById(Guid id);

    Task<GetAerialCensusDto> UpdateAerialCensus(Guid id , AerialCensusRequestDto dto);

    Task<bool> DeleteAerialCensus(Guid id);
}