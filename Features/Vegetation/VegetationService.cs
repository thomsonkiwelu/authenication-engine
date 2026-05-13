using conservation_backend.Features.Files.Interfaces;
using conservation_backend.Features.Vegetation.Interfaces;
using conservation_backend.Shared;
using MapsterMapper;

namespace conservation_backend.Features.Vegetation;

public class VegetationService(IVegetationRepository repository, IFileService fileService, IMapper mapper): IVegetationService
{
    private readonly IVegetationRepository _vegetationRepository = repository;
    private readonly IFileService _fileService = fileService;
    private readonly IMapper _mapper = mapper;
    
    public async Task<PagedList<VegetationResponseDto>> GetPagedVegetations(VegetationPaginationDto dto)
    {
        var pagedData = await _vegetationRepository.GetPagedData(dto);
        
        return new PagedList<VegetationResponseDto>(
            items: pagedData.Data,
            page: pagedData.Page,
            pageSize: pagedData.PageSize,
            totalCount: pagedData.TotalCount
        );
    }

    public async Task<GetVegetationResponseDto> CreateVegetation(VegetationRequestDto dto)
    {
        var vegetationId = await _vegetationRepository.Create(dto);
        
        if (string.IsNullOrWhiteSpace(vegetationId))
            throw new ArgumentNullException("Failure to create vegetation data");
        
        return await _vegetationRepository.GetById(Guid.Parse(vegetationId));
    }

    public async Task<GetVegetationResponseDto> GetVegetationById(Guid id)
    {
        var result = await _vegetationRepository.GetById(id);
        
        result.File = await _fileService.GetSingleFileByEntityData(
            result.Id, AppEntities.VegetationEntity
        );

        return result;
    }

    public async Task<GetVegetationResponseDto> UpdateVegetation(Guid id, VegetationRequestDto dto)
    {
        var vegetationId =  await _vegetationRepository.Update(id, dto);
        
        if (string.IsNullOrWhiteSpace(vegetationId))
            throw new ArgumentNullException("Failure to update vegetation data");
        
        return await _vegetationRepository.GetById(id);
    }

    public async Task<bool> DeleteVegetation(Guid id)
    {
        return await _vegetationRepository.Delete(id);
    }
}