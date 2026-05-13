using conservation_backend.Shared;

namespace conservation_backend.Features.MangabeyObservations.Interfaces;

public interface IMangabeyObservationRepository
{
    Task<PagedList<MangabeyObservation>> GetPagedData(MangabeyObservationPaginationDto dto);

    Task<string> Create(MangabeyObservationRequestDto dto);

    Task<GetMangabeyObservationDto> GetById(Guid id);

    Task<string> Update(Guid id, MangabeyObservationRequestDto dto);

    Task<bool> Delete(Guid id);
}