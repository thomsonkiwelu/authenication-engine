using conservation_backend.Features.Files.Interfaces;
using conservation_backend.Features.LineTransects.Interfaces;
using conservation_backend.Shared;
using MapsterMapper;

namespace conservation_backend.Features.LineTransects;

public class LineTransectService(ILineTransectRepository repository, IFileService fileService, IMapper mapper): ILineTransectService
{
    private readonly ILineTransectRepository _lineTransectRepository = repository;
    private readonly IFileService _fileService = fileService;
    private readonly IMapper _mapper = mapper;
    
    public async Task<PagedList<LineTransectResponseDto>> GetPagedLineTransects(LineTransectPaginationDto dto)
    {
        var pagedData = await _lineTransectRepository.GetPagedData(dto);
        
        return new PagedList<LineTransectResponseDto>(
            items: pagedData.Data,
            page: pagedData.Page,
            pageSize: pagedData.PageSize,
            totalCount: pagedData.TotalCount
        );
    }

    public async Task<GetLineTransectDto> CreateLineTransect(LineTransectRequestDto dto)
    {
        var lineTransectId = await _lineTransectRepository.Create(dto);
        
        if (string.IsNullOrWhiteSpace(lineTransectId))
            throw new ArgumentNullException("Failure to create line transect data");
        
        return await _lineTransectRepository.GetById(Guid.Parse(lineTransectId));
    }

    public async Task<GetLineTransectDto> GetLineTransectById(Guid id)
    {
        var result = await _lineTransectRepository.GetById(id);

        foreach (var lineTransect in result.LineTransectMigratoryBirds)
        {
            lineTransect.File = await _fileService.GetSingleFileByEntityData(
                lineTransect.Id, AppEntities.MigratoryBirdEntity
            );
        }
        
        foreach (var alongTransect in result.AlongTransectMigratoryBirds)
        {
            alongTransect.File = await _fileService.GetSingleFileByEntityData(
                alongTransect.Id, AppEntities.MigratoryBirdEntity
            );
        }

        return result;
    }

    public async Task<GetLineTransectDto> UpdateLineTransect(Guid id, LineTransectRequestDto dto)
    {
        var lineTransectId =  await _lineTransectRepository.Update(id, dto);
        
        if (string.IsNullOrWhiteSpace(lineTransectId))
            throw new ArgumentNullException("Failure to update line transect data");
        
        return await _lineTransectRepository.GetById(Guid.Parse(lineTransectId));
    }

    public async Task<bool> DeleteLineTransect(Guid id)
    {
        return await _lineTransectRepository.Delete(id);
    }
    
}