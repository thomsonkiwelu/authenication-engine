using conservation_backend.Features.Files.Interfaces;
using conservation_backend.Features.VillageProfiles.Interfaces;
using conservation_backend.Shared;
using MapsterMapper;

namespace conservation_backend.Features.VillageProfiles;

public class VillageProfileService(IVillageProfileRepository repository, IFileService fileService, IMapper mapper) : IVillageProfileService
{
    private readonly IVillageProfileRepository _villageProfileRepository = repository;
    private readonly IFileService _fileService = fileService;
    private readonly IMapper _mapper = mapper;
    
    public async Task<PagedList<VillageProfileResponseDto>> GetAllVillageProfilesData(VillageProfilePaginationDto dto)
    {
        var pagedData = await _villageProfileRepository.GetPagedData(dto);
        var startIndex = (pagedData.Page - 1) * pagedData.PageSize + 1;

        var dtoList = _mapper.Map<List<VillageProfileResponseDto>>(pagedData.Data)
            .Select((dto, index) => dto with { RowNumber = startIndex + index }).ToList();

        return new PagedList<VillageProfileResponseDto>(
            items: dtoList,
            page: pagedData.Page,
            pageSize: pagedData.PageSize,
            totalCount: pagedData.TotalCount
        );
    }

    public async Task<VillageProfileDto> CreateVillageProfile(VillageProfileRequestDto dto)
    {
        var villageProfile = _mapper.Map<VillageProfile>(dto);
        var created = await _villageProfileRepository.Create(villageProfile);
        
        // Update Image data
        if (!string.IsNullOrWhiteSpace(dto.ImageId))
            await _fileService.UpdateUploadedFile(dto.ImageId, AppEntities.VillageProfileEntity, created.Id);
        
        var responseDto = _mapper.Map<VillageProfileDto>(created);
        return responseDto;
    }

    public async Task<VillageProfileDto> GetVillageProfileById(Guid id)
    {
        var villageProfile = await _villageProfileRepository.GetById(id);
        var result = _mapper.Map<VillageProfileDto>(villageProfile);
        
        result.File = await _fileService.GetSingleFileByEntityData(
            id, AppEntities.VillageProfileEntity
        );
        
        return result;
    }

    public async Task<VillageProfileDto> UpdateVillageProfile(Guid id, VillageProfileRequestDto dto)
    {
        var villageProfile = _mapper.Map<VillageProfile>(dto);

        var updated = await _villageProfileRepository.Update(id, villageProfile);

        var responseDto = _mapper.Map<VillageProfileDto>(updated);

        return responseDto;
    }

    public async Task<bool> DeleteVillageProfile(Guid id)
    {
        return await _villageProfileRepository.Delete(id);
    }
    
    public async Task<GetVillageProfileDto> GetFullVillageProfileById(Guid id)
    {
        var result = await _villageProfileRepository.GetFullDetails(id);
        
        result.File = await _fileService.GetSingleFileByEntityData(
            id, AppEntities.VillageProfileEntity
        );
        
        return result;
    }
}