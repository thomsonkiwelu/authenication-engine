using conservation_backend.Shared;

namespace conservation_backend.Features.Units.Interfaces
{
    public interface IUnitRepository
    {
        Task<PagedList<Unit>> GetPagedData(UnitPaginationDto dto);

        Task<Unit> Create(Unit unit);

        Task<Unit> GetById(Guid id);

        Task<Unit> Update(Guid id, Unit unit);

        Task<bool> Delete(Guid id);
        
        Task<UnitsWithDepartmentAndSectionDto> GetUnitsWithDepartmentAndSection(Guid officeId);
    }
}
