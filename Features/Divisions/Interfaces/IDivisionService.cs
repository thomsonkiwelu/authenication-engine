using conservation_backend.Shared;

namespace conservation_backend.Features.Divisions.Interfaces;

public interface IDivisionService
{
    Task<PagedList<DivisionResponseDto>> GetAllDivisions(DivisionPaginationDto dto);

    Task<DivisionDto> CreateDivision(DivisionRequest dto);

    Task<DivisionDto> GetDivisionById(Guid id);

    Task<DivisionDto> UpdateDivision(Guid id, DivisionRequest dto);

    Task<bool> DeleteDivision(Guid id);
}