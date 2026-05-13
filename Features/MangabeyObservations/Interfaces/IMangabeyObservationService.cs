using conservation_backend.Shared;

namespace conservation_backend.Features.MangabeyObservations.Interfaces;

public interface IMangabeyObservationService
{
    Task<PagedList<MangabeyObservationResponseDto>> GetMangabeyObservations(MangabeyObservationPaginationDto dto);
    
    Task<GetMangabeyObservationDto> CreateMangabeyObservation(MangabeyObservationRequestDto dto);
    
    Task<GetMangabeyObservationDto> GetMangabeyObservationById(Guid id);

    Task<GetMangabeyObservationDto> UpdateMangabeyObservation(Guid id , MangabeyObservationRequestDto dto);

    Task<bool> DeleteMangabeyObservation(Guid id);
}