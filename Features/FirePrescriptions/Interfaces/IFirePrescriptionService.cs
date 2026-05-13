using conservation_backend.Shared;

namespace conservation_backend.Features.FirePrescriptions.Interfaces;

public interface IFirePrescriptionService
{
    Task<PagedList<FirePrescriptionResponseDto>> GetFirePrescriptions(FirePrescriptionPaginationDto dto);
    
    Task<GetFirePrescriptionDto> CreateFirePrescription(FirePrescriptionRequestDto dto);
    
    Task<GetFirePrescriptionDto> GetFirePrescriptionById(Guid id);

    Task<GetFirePrescriptionDto> UpdateFirePrescription(Guid id , FirePrescriptionRequestDto dto);

    Task<bool> DeleteFirePrescription(Guid id);
}