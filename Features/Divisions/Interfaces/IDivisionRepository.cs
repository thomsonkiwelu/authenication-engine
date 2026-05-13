using conservation_backend.Shared;

namespace conservation_backend.Features.Divisions.Interfaces;

public interface IDivisionRepository
{
    Task<PagedList<Division>> GetPagedData(DivisionPaginationDto dto);

    Task<Division> Create(Division division);

    Task<Division> GetById(Guid id);

    Task<Division> Update(Guid id, Division division);

    Task<bool> Delete(Guid id);
}