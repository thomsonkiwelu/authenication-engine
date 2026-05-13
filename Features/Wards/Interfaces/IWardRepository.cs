using conservation_backend.Shared;

namespace conservation_backend.Features.Wards.Interfaces;

public interface IWardRepository
{
    Task<PagedList<Ward>> GetPagedData(WardPaginationDto dto);

    Task<Ward> Create(Ward ward);

    Task<Ward> GetById(Guid id);

    Task<Ward> Update(Guid id, Ward ward);

    Task<bool> Delete(Guid id);
}