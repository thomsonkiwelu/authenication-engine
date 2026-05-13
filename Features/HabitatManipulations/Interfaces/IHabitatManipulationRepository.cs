using conservation_backend.Shared;

namespace conservation_backend.Features.HabitatManipulations.Interfaces;

public interface IHabitatManipulationRepository
{
    Task<PagedList<HabitatManipulationResponseDto>> GetPagedData(HabitatManipulationPaginationDto dto);

    Task<string> Create(HabitatManipulationRequestDto dto);

    Task<GetHabitatManipulationDto> GetById(Guid id);

    Task<string> Update(Guid id, HabitatManipulationRequestDto dto);

    Task<bool> Delete(Guid id);
}