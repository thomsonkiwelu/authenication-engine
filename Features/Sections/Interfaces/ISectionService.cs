using conservation_backend.Shared;

namespace conservation_backend.Features.Sections.Interfaces
{
    public interface ISectionService
    {
        Task<PagedList<SectionResponseDto>> GetAllSectionsData(SectionPaginationDto dto);

        Task<SectionDto> CreateSection(SectionRequest dto);

        Task<SectionWithDepartmentDto> GetSectionById(Guid id);

        Task<SectionDto> UpdateSection(Guid id, SectionRequest dto);

        Task<bool> DeleteSection(Guid id);
        
        Task<SectionAndDepartmentDto> GetSectionsAndDepartment(Guid officeId);
    }
}
