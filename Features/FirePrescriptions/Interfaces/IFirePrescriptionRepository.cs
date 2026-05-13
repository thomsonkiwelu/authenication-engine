using conservation_backend.Shared;

namespace conservation_backend.Features.FirePrescriptions.Interfaces;

public interface IFirePrescriptionRepository
{
    Task<PagedList<FirePrescription>> GetPagedData(FirePrescriptionPaginationDto dto);

    Task<string> Create(FirePrescriptionRequestDto dto);

    Task<GetFirePrescriptionDto> GetById(Guid id);

    Task<string> Update(Guid id, FirePrescriptionRequestDto dto);

    Task<bool> Delete(Guid id);
}