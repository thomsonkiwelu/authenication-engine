using conservation_backend.Features.Files.Interfaces;
using conservation_backend.Features.WaterQuantities.Interfaces;
using conservation_backend.Shared;
using MapsterMapper;

namespace conservation_backend.Features.WaterQuantities;

public class WaterQuantityService(IWaterQuantityRepository repository, IFileService fileService, IMapper mapper): IWaterQuantityService
{
    private readonly IWaterQuantityRepository _waterQuantityRepository = repository;
    private readonly IFileService _fileService = fileService;
    private readonly IMapper _mapper = mapper;
    
    
    public async Task<PagedList<WaterQuantityResponseDto>> GetAllWaterQuantitiesData(WaterQuantityPaginationDto dto)
    {
        var pagedData = await _waterQuantityRepository.GetPagedData(dto);
        var startIndex = (pagedData.Page - 1) * pagedData.PageSize + 1;

        var dtoList = _mapper.Map<List<WaterQuantityResponseDto>>(pagedData.Data)
            .Select((dto, index) => dto with { RowNumber = startIndex + index }).ToList();

        return new PagedList<WaterQuantityResponseDto>(
            items: dtoList,
            page: pagedData.Page,
            pageSize: pagedData.PageSize,
            totalCount: pagedData.TotalCount
        );
    }

    public async Task<WaterQuantityDto> CreateWaterQuantity(WaterQuantityRequestDto dto)
    {
        var waterQuantity = _mapper.Map<WaterQuantity>(dto);
        var created = await _waterQuantityRepository.Create(waterQuantity);
        
        // Update File data
        if (!string.IsNullOrWhiteSpace(dto.FileAttachmentId))
            await _fileService.UpdateUploadedFile(dto.FileAttachmentId, AppEntities.WaterQuantityEntity, created.Id);
        
        // Update Image data
        if (!string.IsNullOrWhiteSpace(dto.ImageId))
            await _fileService.UpdateUploadedFile(dto.ImageId, AppEntities.WaterQuantityEntity, created.Id);
            
        var responseDto = _mapper.Map<WaterQuantityDto>(created);
        return responseDto;
    }

    public async Task<WaterQuantityDto> GetWaterQuantityById(Guid id)
    {
        var waterQuantity = await _waterQuantityRepository.GetById(id);
        var result = _mapper.Map<WaterQuantityDto>(waterQuantity);

        result.Attachments = await _fileService.GetFileByEntityData(
            id, AppEntities.WaterQuantityEntity
            );
        
        return result;
    }

    public async Task<WaterQuantityDto> UpdateWaterQuantity(Guid id, WaterQuantityRequestDto dto)
    {
        var waterQuantity = _mapper.Map<WaterQuantity>(dto);
        var updated = await _waterQuantityRepository.Update(id, waterQuantity);
        var responseDto = _mapper.Map<WaterQuantityDto>(updated);
        
        return responseDto;
    }

    public async Task<bool> DeleteWaterQuantity(Guid id)
    {
        return await _waterQuantityRepository.Delete(id);
    }
}