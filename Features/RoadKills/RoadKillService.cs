using conservation_backend.Features.Files.Interfaces;
using conservation_backend.Features.RoadKills.Interfaces;
using conservation_backend.Shared;
using MapsterMapper;

namespace conservation_backend.Features.RoadKills;

public class RoadKillService(IRoadKillRepository repository, IFileService fileService, IMapper mapper): IRoadKillService
{
    private readonly IRoadKillRepository _roadKillRepository = repository;
    private readonly IFileService _fileService = fileService;
    private readonly IMapper _mapper = mapper;
    
    public async Task<PagedList<RoadKillResponseDto>> GetPagedRoadKills(RoadKillPaginationDto dto)
    {
        var pagedData = await _roadKillRepository.GetPagedData(dto);
        
        return new PagedList<RoadKillResponseDto>(
            items: pagedData.Data,
            page: pagedData.Page,
            pageSize: pagedData.PageSize,
            totalCount: pagedData.TotalCount
        );
    }

    public async Task<GetRoadKillDto> CreateRoadKill(RoadKillRequestDto dto)
    {
        var roadKillId = await _roadKillRepository.Create(dto);
        
        if (string.IsNullOrWhiteSpace(roadKillId))
            throw new ArgumentNullException("Failure to create road kill data");
        
        return await _roadKillRepository.GetById(Guid.Parse(roadKillId));
    }

    public async Task<GetRoadKillDto> GetRoadKillById(Guid id)
    {
        var result = await _roadKillRepository.GetById(id);
        
        result.File = await _fileService.GetSingleFileByEntityData(
            result.Id, AppEntities.RoadKillEntity
        );

        return result;
    }

    public async Task<GetRoadKillDto> UpdateRoadKill(Guid id, RoadKillRequestDto dto)
    {
        var roadKillId =  await _roadKillRepository.Update(id, dto);
        
        if (string.IsNullOrWhiteSpace(roadKillId))
            throw new ArgumentNullException("Failure to update road kill data");
        
        return await _roadKillRepository.GetById(Guid.Parse(roadKillId));
    }

    public async Task<bool> DeleteRoadKill(Guid id)
    {
        return await _roadKillRepository.Delete(id);
    }
}