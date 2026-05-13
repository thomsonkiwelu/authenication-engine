using authentication_engine.Shared;

namespace authentication_engine.Features.Units.Interfaces
{
    public interface IUnitService
    {
        Task<PagedList<UnitResponseDto>> GetAllUnitsData(UnitPaginationDto dto);

        Task<UnitDto> CreateUnit(UnitRequest dto);

        Task<UnitDto> GetUnitById(Guid id);

        Task<UnitDto> UpdateUnit(Guid id, UnitRequest dto);

        Task<bool> DeleteUnit(Guid id);
        
        Task<UnitsWithDepartmentAndSectionDto> GetUnitsWithDepartmentAndSection(Guid officeId);
    }
}
