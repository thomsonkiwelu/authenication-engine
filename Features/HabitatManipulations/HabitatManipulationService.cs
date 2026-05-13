using conservation_backend.Features.HabitatManipulations.Interfaces;
using conservation_backend.Shared;
using MapsterMapper;

namespace conservation_backend.Features.HabitatManipulations;

public class HabitatManipulationService(IHabitatManipulationRepository repository, IMapper mapper): IHabitatManipulationService
{
    private readonly IHabitatManipulationRepository _habitatManipulationRepository = repository;
    private readonly IMapper _mapper = mapper;
    
    public async Task<PagedList<HabitatManipulationResponseDto>> GetPagedHabitatManipulations(HabitatManipulationPaginationDto dto)
    {
        var pagedData = await _habitatManipulationRepository.GetPagedData(dto);
        
        return new PagedList<HabitatManipulationResponseDto>(
            items: pagedData.Data,
            page: pagedData.Page,
            pageSize: pagedData.PageSize,
            totalCount: pagedData.TotalCount
        );
    }

    public async Task<GetHabitatManipulationDto> CreateHabitatManipulation(HabitatManipulationRequestDto dto)
    {
        var habitatManipulationId = await _habitatManipulationRepository.Create(dto);
        
        if (string.IsNullOrWhiteSpace(habitatManipulationId))
            throw new ArgumentNullException("Failure to create habitat manipulation data");
        
        return await _habitatManipulationRepository.GetById(Guid.Parse(habitatManipulationId));
    }

    public async Task<GetHabitatManipulationDto> GetHabitatManipulationById(Guid id)
    {
        return await _habitatManipulationRepository.GetById(id);
    }

    public async Task<GetHabitatManipulationDto> UpdateHabitatManipulation(Guid id, HabitatManipulationRequestDto dto)
    {
        var habitatManipulationId = await _habitatManipulationRepository.Update(id, dto);
        
        if (string.IsNullOrWhiteSpace(habitatManipulationId))
            throw new ArgumentNullException("Failure to update habitat manipulation data");
        
        return await _habitatManipulationRepository.GetById(Guid.Parse(habitatManipulationId));
    }

    public async Task<bool> DeleteHabitatManipulation(Guid id)
    {
        return await _habitatManipulationRepository.Delete(id);
    }
}