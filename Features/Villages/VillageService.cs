using conservation_backend.Features.Villages.Interfaces;
using conservation_backend.Shared;
using MapsterMapper;

namespace conservation_backend.Features.Villages;

public class VillageService(IVillageRepository repository, IMapper mapper): IVillageService
{
    private readonly IVillageRepository _villageRepository = repository;
    private readonly IMapper _mapper = mapper;
    
    public async Task<PagedList<VillageResponseDto>> GetAllVillages(VillagePaginationDto dto)
    {
        var pagedData = await _villageRepository.GetPagedData(dto);
        var startIndex = (pagedData.Page - 1) * pagedData.PageSize + 1;

        var dtoList = _mapper.Map<List<VillageResponseDto>>(pagedData.Data)
            .Select((dto, index) => dto with { RowNumber = startIndex + index }).ToList();

        return new PagedList<VillageResponseDto>(
            items: dtoList,
            page: pagedData.Page,
            pageSize: pagedData.PageSize,
            totalCount: pagedData.TotalCount
        );
    }

    public async Task<VillageDto> CreateVillage(VillageRequest dto)
    {
        var village = _mapper.Map<Village>(dto);
        var result = await _villageRepository.Create(village);
        
        var responseDto = _mapper.Map<VillageDto>(result);
        return responseDto;
    }

    public async Task<VillageDto> GetVillageById(Guid id)
    {
        var village = await _villageRepository.GetById(id);
        
        var result = _mapper.Map<VillageDto>(village);
        return result;
    }

    public async Task<VillageDto> UpdateVillage(Guid id, VillageRequest dto)
    {
        var village = _mapper.Map<Village>(dto);
        var updated = await _villageRepository.Update(id, village);

        var responseDto = _mapper.Map<VillageDto>(updated);
        return responseDto;
    }

    public async Task<bool> DeleteVillage(Guid id)
    {
        return await _villageRepository.Delete(id);
    }
}