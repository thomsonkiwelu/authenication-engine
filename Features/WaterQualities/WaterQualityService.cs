using conservation_backend.Features.Files.Interfaces;
using conservation_backend.Features.WaterBodies.Interfaces;
using conservation_backend.Features.WaterQualities.Interfaces;
using conservation_backend.Shared;
using MapsterMapper;

namespace conservation_backend.Features.WaterQualities;

public class WaterQualityService(IWaterQualityRepository repository, IFileService fileService, IMapper mapper): IWaterQualityService
{
    private readonly IWaterQualityRepository _waterQualityRepository = repository;
    private readonly IFileService _fileService = fileService;
    private readonly IMapper _mapper = mapper;
    
    public async Task<PagedList<WaterQualityResponseDto>> GetAllWaterQualitiesData(WaterQualityPaginationDto dto)
    {
        var pagedData = await _waterQualityRepository.GetPagedData(dto);
        var startIndex = (pagedData.Page - 1) * pagedData.PageSize + 1;

        var dtoList = _mapper.Map<List<WaterQualityResponseDto>>(pagedData.Data)
            .Select((dto, index) => dto with { RowNumber = startIndex + index }).ToList();

        return new PagedList<WaterQualityResponseDto>(
            items: dtoList,
            page: pagedData.Page,
            pageSize: pagedData.PageSize,
            totalCount: pagedData.TotalCount
        );
    }

    public async Task<WaterQualityDto> CreateWaterQuality(WaterQualityRequestDto dto)
    {
        var waterQuality = _mapper.Map<WaterQuality>(dto);
        var created = await _waterQualityRepository.Create(waterQuality);
        
        if(!string.IsNullOrWhiteSpace(dto.ImageId))
            await _fileService.UpdateUploadedFile(dto.ImageId, AppEntities.WaterQualityEntity, created.Id);
        
        var responseDto = _mapper.Map<WaterQualityDto>(created);
        return responseDto;
    }

    public async Task<WaterQualityDto> GetWaterQualityById(Guid id)
    {
        var waterQuality = await _waterQualityRepository.GetById(id);
        
        var result = _mapper.Map<WaterQualityDto>(waterQuality);
        
        result.File = await _fileService.GetSingleFileByEntityData(
            result.Id, AppEntities.WaterQualityEntity
        );
        
        return result;
    }

    public async Task<WaterQualityDto> UpdateWaterQuality(Guid id, WaterQualityRequestDto dto)
    {
        var waterQuality = _mapper.Map<WaterQuality>(dto);
        var updated = await _waterQualityRepository.Update(id, waterQuality);
        var responseDto = _mapper.Map<WaterQualityDto>(updated);
        
        return responseDto;
    }

    public async Task<bool> DeleteWaterQuality(Guid id)
    {
        return await _waterQualityRepository.Delete(id);
    }
}