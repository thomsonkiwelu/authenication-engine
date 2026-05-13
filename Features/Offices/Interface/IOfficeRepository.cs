using conservation_backend.Shared;

namespace conservation_backend.Features.Offices.Interface
{
    public interface IOfficeRepository
    {
        Task<PagedList<Office>> GetPagedData(OfficePaginationDto dto);

        Task<Office> Create(Office office);

        Task<Office> GetById(Guid id);

        Task<Office> Update(Guid id, Office office);

        Task<bool> Delete(Guid id);
    }
}
