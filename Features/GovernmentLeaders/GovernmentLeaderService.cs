using conservation_backend.Features.Files.Interfaces;
using conservation_backend.Features.GovernmentLeaders.Interfaces;
using conservation_backend.Shared;
using MapsterMapper;

namespace conservation_backend.Features.GovernmentLeaders;

public class GovernmentLeaderService(IGovernmentLeaderRepository repository, IMapper mapper) : IGovernmentLeaderService
{
    private readonly IGovernmentLeaderRepository _governmentLeaderRepository = repository;
    private readonly IMapper _mapper = mapper;
    
    public async Task<PagedList<GovernmentLeaderResponseDto>> GetAllGovernmentLeaders(GovernmentLeaderPaginationDto dto)
    {
        var pagedData = await _governmentLeaderRepository.GetPagedData(dto);
        var startIndex = (pagedData.Page - 1) * pagedData.PageSize + 1;

        var dtoList = _mapper.Map<List<GovernmentLeaderResponseDto>>(pagedData.Data)
            .Select((dto, index) => dto with { RowNumber = startIndex + index }).ToList();

        return new PagedList<GovernmentLeaderResponseDto>(
            items: dtoList,
            page: pagedData.Page,
            pageSize: pagedData.PageSize,
            totalCount: pagedData.TotalCount
        );
    }

    public async Task<GovernmentLeaderDto> CreateGovernmentLeader(GovernmentLeaderRequestDto dto)
    {
        var governmentLeader = _mapper.Map<GovernmentLeader>(dto);
        
        var created = await _governmentLeaderRepository.Create(governmentLeader);
        
        var responseDto = _mapper.Map<GovernmentLeaderDto>(created);
        return responseDto;
    }

    public async Task<GovernmentLeaderDto> GetGovernmentLeaderById(Guid id)
    {
        var governmentLeader = await _governmentLeaderRepository.GetById(id);
        
        var result = _mapper.Map<GovernmentLeaderDto>(governmentLeader);
        
        return result;
    }

    public async Task<GovernmentLeaderDto> UpdateGovernmentLeader(Guid id, GovernmentLeaderRequestDto dto)
    {
        var governmentLeader = _mapper.Map<GovernmentLeader>(dto);

        var updated = await _governmentLeaderRepository.Update(id, governmentLeader);

        var responseDto = _mapper.Map<GovernmentLeaderDto>(updated);

        return responseDto;
    }

    public async Task<bool> DeleteGovernmentLeader(Guid id)
    {
        return await _governmentLeaderRepository.Delete(id);
    }
}