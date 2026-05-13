using conservation_backend.Features.DistrictProfiles.Interfaces;
using conservation_backend.Features.Files.Interfaces;
using conservation_backend.Shared;
using MapsterMapper;

namespace conservation_backend.Features.DistrictProfiles;

public class DistrictProfileService(IDistrictProfileRepository repository, IFileService fileService, IMapper mapper): IDistrictProfileService
{
    private readonly IDistrictProfileRepository _districtProfileRepository = repository;
    private readonly IFileService _fileService = fileService;
    private readonly IMapper _mapper = mapper;

    public async Task<PagedList<DistrictProfileResponseDto>> GetDistrictProfiles(DistrictProfilePaginationDto dto)
    {
        var pagedData = await _districtProfileRepository.GetPagedData(dto);

        var startIndex = (pagedData.Page - 1) * pagedData.PageSize + 1;

        var dtoList = _mapper.Map<List<DistrictProfileResponseDto>>(pagedData.Data)
            .Select((dto, index) => dto with { RowNumber = startIndex + index }).ToList();

        return new PagedList<DistrictProfileResponseDto>(
            items: dtoList,
            page: pagedData.Page,
            pageSize: pagedData.PageSize,
            totalCount: pagedData.TotalCount
        );
    }

    public async Task<GetDistrictProfileDto> CreateDistrictProfile(DistrictProfileRequestDto dto)
    {
        var districtProfileId = await _districtProfileRepository.Create(dto);
        
        if (string.IsNullOrWhiteSpace(districtProfileId))
            throw new ArgumentNullException("Failure to create district profile data");
        
        return await _districtProfileRepository.GetById(Guid.Parse(districtProfileId));
    }

    public async Task<GetDistrictProfileDto> GetDistrictProfileById(Guid id)
    {
        var result = await _districtProfileRepository.GetById(id);

        result.File = await _fileService.GetSingleFileByEntityData(
                result.Id, AppEntities.DistrictProfileEntity
            );

        return result;
    }

    public async Task<GetDistrictProfileDto> UpdateDistrictProfile(Guid id, DistrictProfileRequestDto dto)
    {
        var districtProfileId =  await _districtProfileRepository.Update(id, dto);
        
        if (string.IsNullOrWhiteSpace(districtProfileId))
            throw new ArgumentNullException("Failure to update district profile data");
        
        return await _districtProfileRepository.GetById(Guid.Parse(districtProfileId));
    }

    public async Task<bool> DeleteDistrictProfile(Guid id)
    {
        return await _districtProfileRepository.Delete(id);
    }
    
    public async Task<GetFullDistrictProfileDto> GetFullDistrictProfileById(Guid id)
    {
        var result = await _districtProfileRepository.GetFullDetails(id);
        
        result.File = await _fileService.GetSingleFileByEntityData(
            id, AppEntities.DistrictProfileEntity
        );
        
        return result;
    }
}