using authentication_engine.Shared;

namespace authentication_engine.Features.Sections.Interfaces
{
    public interface ISectionRepository
    {
        Task<PagedList<Section>> GetPagedData(SectionPaginationDto dto);

        Task<Section> Create(Section section);

        Task<Section> GetById(Guid id);

        Task<Section> Update(Guid id, Section section);

        Task<bool> Delete(Guid id);
        
        Task<SectionAndDepartmentDto> GetSectionsAndDepartment(Guid officeId);
    }
}
