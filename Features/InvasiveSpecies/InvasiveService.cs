using conservation_backend.Features.Files.Interfaces;
using conservation_backend.Features.InvasiveSpecies.Interfaces;
using conservation_backend.Shared;
using MapsterMapper;

namespace conservation_backend.Features.InvasiveSpecies;

public class InvasiveService(IInvasiveRepository repository, IFileService fileService, IMapper mapper): IInvasiveService
{
    private readonly IInvasiveRepository _invasiveRepository = repository;
    private readonly IFileService _fileService = fileService;
    private readonly IMapper _mapper = mapper;
    
    public async Task<PagedList<InvasiveResponseDto>> GetPagedInvasiveSpecies(InvasivePaginationDto dto)
    {
        var pagedData = await _invasiveRepository.GetPagedData(dto);
        
        return new PagedList<InvasiveResponseDto>(
            items: pagedData.Data,
            page: pagedData.Page,
            pageSize: pagedData.PageSize,
            totalCount: pagedData.TotalCount
        );
    }

    public async Task<GetInvasiveSpeciesDto> CreateInvasiveSpecies(InvasiveRequestDto dto)
    {
        var invasiveId = await _invasiveRepository.Create(dto);
        
        if (string.IsNullOrWhiteSpace(invasiveId))
            throw new ArgumentNullException("Failure to create invasive species data");
        
        return await _invasiveRepository.GetById(Guid.Parse(invasiveId));
    }

    public async Task<GetInvasiveSpeciesDto> GetInvasiveSpeciesById(Guid id)
    {
        var result = await _invasiveRepository.GetById(id);
        
        result.File = await _fileService.GetSingleFileByEntityData(
            result.Id, AppEntities.InvasiveSpeciesEntity
        );

        return result;
    }

    public async Task<GetInvasiveSpeciesDto> UpdateInvasiveSpecies(Guid id, InvasiveRequestDto dto)
    {
        var invasiveId =  await _invasiveRepository.Update(id, dto);
        
        if (string.IsNullOrWhiteSpace(invasiveId))
            throw new ArgumentNullException("Failure to update invasive species data");
        
        return await _invasiveRepository.GetById(id);
    }

    public async Task<bool> DeleteInvasiveSpecies(Guid id)
    {
        return await _invasiveRepository.Delete(id);
    }
    
}