using conservation_backend.Shared;

namespace conservation_backend.Features.HabitatManipulations.Interfaces;

public interface IHabitatManipulationService
{
    Task<PagedList<HabitatManipulationResponseDto>> GetPagedHabitatManipulations(HabitatManipulationPaginationDto dto);
    
    Task<GetHabitatManipulationDto> CreateHabitatManipulation(HabitatManipulationRequestDto dto);
    
    Task<GetHabitatManipulationDto> GetHabitatManipulationById(Guid id);

    Task<GetHabitatManipulationDto> UpdateHabitatManipulation(Guid id , HabitatManipulationRequestDto dto);

    Task<bool> DeleteHabitatManipulation(Guid id);
}