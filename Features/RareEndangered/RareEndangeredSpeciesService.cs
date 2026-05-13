using conservation_backend.Features.Files.Interfaces;
using conservation_backend.Features.RareEndangered.Interfaces;
using conservation_backend.Shared;
using MapsterMapper;

namespace conservation_backend.Features.RareEndangered;

public class RareEndangeredSpeciesService(IRareEndangeredSpeciesRepository repository, IFileService fileService, IMapper mapper): IRareEndangeredSpeciesService
{
    private readonly IRareEndangeredSpeciesRepository _rareEndangeredSpeciesRepository = repository;
    private readonly IFileService _fileService = fileService;
    private readonly IMapper _mapper = mapper;
    
    public async Task<PagedList<RareEndangeredSpeciesResponseDto>> GetPagedRareEndangeredSpecies(RareEndangeredSpeciesPaginationDto dto)
    {
        var pagedData = await _rareEndangeredSpeciesRepository.GetPagedData(dto);
        
        return new PagedList<RareEndangeredSpeciesResponseDto>(
            items: pagedData.Data,
            page: pagedData.Page,
            pageSize: pagedData.PageSize,
            totalCount: pagedData.TotalCount
        );
    }

    public async Task<GetRareEndangeredSpeciesDto> CreateRareEndangeredSpecies(RareEndangeredSpeciesRequestDto dto)
    {
        var rareEndangeredSpeciesId = await _rareEndangeredSpeciesRepository.Create(dto);
        
        if (string.IsNullOrWhiteSpace(rareEndangeredSpeciesId))
            throw new ArgumentNullException("Failure to add rare endangered species data");
        
        return await _rareEndangeredSpeciesRepository.GetById(Guid.Parse(rareEndangeredSpeciesId));
    }

    public async Task<GetRareEndangeredSpeciesDto> GetRareEndangeredSpeciesById(Guid id)
    {
        var result = await _rareEndangeredSpeciesRepository.GetById(id);

        foreach (var rareSpecies in result.RareSpeciesOccurrences)
        {
            rareSpecies.File = await _fileService.GetSingleFileByEntityData(
                rareSpecies.Id, AppEntities.RareSpeciesOccurrenceEntity
            );
        }
        return result;
    }

    public async Task<GetRareEndangeredSpeciesDto> UpdateRareEndangeredSpecies(Guid id, RareEndangeredSpeciesRequestDto dto)
    {
        var rareEndangeredSpeciesId =  await _rareEndangeredSpeciesRepository.Update(id, dto);
        
        if (string.IsNullOrWhiteSpace(rareEndangeredSpeciesId))
            throw new ArgumentNullException("Failure to update rare endangered species data");
        
        return await _rareEndangeredSpeciesRepository.GetById(Guid.Parse(rareEndangeredSpeciesId));
    }

    public async Task<bool> DeleteRareEndangeredSpecies(Guid id)
    {
        return await _rareEndangeredSpeciesRepository.Delete(id);
    }
}