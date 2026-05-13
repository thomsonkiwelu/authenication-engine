using conservation_backend.Features.Files.Interfaces;
using conservation_backend.Features.FirePrescriptions.Interfaces;
using conservation_backend.Shared;
using MapsterMapper;

namespace conservation_backend.Features.FirePrescriptions;

public class FirePrescriptionService(IFirePrescriptionRepository repository, IFileService fileService, IMapper mapper): IFirePrescriptionService
{
    private readonly IFirePrescriptionRepository _firePrescriptionRepository = repository;
    private readonly IFileService _fileService = fileService;
    private readonly IMapper _mapper = mapper;

    public async Task<PagedList<FirePrescriptionResponseDto>> GetFirePrescriptions(FirePrescriptionPaginationDto dto)
    {
        var pagedData = await _firePrescriptionRepository.GetPagedData(dto);
        var startIndex = (pagedData.Page - 1) * pagedData.PageSize + 1;

        var dtoList = _mapper.Map<List<FirePrescriptionResponseDto>>(pagedData.Data)
            .Select((dto, index) => dto with { RowNumber = startIndex + index }).ToList();

        return new PagedList<FirePrescriptionResponseDto>(
            items: dtoList,
            page: pagedData.Page,
            pageSize: pagedData.PageSize,
            totalCount: pagedData.TotalCount
        );
    }

    public async Task<GetFirePrescriptionDto> CreateFirePrescription(FirePrescriptionRequestDto dto)
    {
        var firePrescriptionId = await _firePrescriptionRepository.Create(dto);
        
        if (string.IsNullOrWhiteSpace(firePrescriptionId))
            throw new ArgumentNullException("Failure to create fire prescription data");
        
        // Update Image data
        if (!string.IsNullOrWhiteSpace(dto.ImageId))
            await _fileService.UpdateUploadedFile(dto.ImageId, AppEntities.FirePrescriptionEntity, Guid.Parse(firePrescriptionId));

        return await _firePrescriptionRepository.GetById(Guid.Parse(firePrescriptionId));
    }

    public async Task<GetFirePrescriptionDto> GetFirePrescriptionById(Guid id)
    {
        var result = await _firePrescriptionRepository.GetById(id);
        
        result.File = await _fileService.GetFileByEntityData(
            id, AppEntities.FirePrescriptionEntity
        );
        
        return result;
    }

    public async Task<GetFirePrescriptionDto> UpdateFirePrescription(Guid id, FirePrescriptionRequestDto dto)
    {
        var firePrescriptionId = await _firePrescriptionRepository.Update(id, dto);
        
        if (string.IsNullOrWhiteSpace(firePrescriptionId))
            throw new ArgumentNullException("Failure to update fire prescription data");
        
        return await _firePrescriptionRepository.GetById(Guid.Parse(firePrescriptionId));
    }

    public async Task<bool> DeleteFirePrescription(Guid id)
    {
        return await _firePrescriptionRepository.Delete(id);
    }
}