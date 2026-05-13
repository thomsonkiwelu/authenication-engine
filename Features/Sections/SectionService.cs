using conservation_backend.Features.Sections.Interfaces;
using conservation_backend.Shared;
using MapsterMapper;

namespace conservation_backend.Features.Sections
{
    public class SectionService(ISectionRepository repository, IMapper mapper) : ISectionService
    {
        private readonly ISectionRepository _sectionRepository = repository;

        private readonly IMapper _mapper = mapper;

        public async Task<PagedList<SectionResponseDto>> GetAllSectionsData(SectionPaginationDto dto)
        {
            var pagedData = await _sectionRepository.GetPagedData(dto);

            var startIndex = (pagedData.Page - 1) * pagedData.PageSize + 1;

            var dtoList = _mapper.Map<List<SectionResponseDto>>(pagedData.Data)
                .Select((dto, index) => dto with { RowNumber = startIndex + index }).ToList();

            return new PagedList<SectionResponseDto>(
                items: dtoList,
                page: pagedData.Page,
                pageSize: pagedData.PageSize,
                totalCount: pagedData.TotalCount
            );
        }

        public async Task<SectionDto> CreateSection(SectionRequest dto)
        {
            var section = _mapper.Map<Section>(dto);

            var createdSection = await _sectionRepository.Create(section);

            var responseDto = _mapper.Map<SectionDto>(createdSection);

            return responseDto;
        }

        public async Task<SectionWithDepartmentDto> GetSectionById(Guid id)
        {
            var section = await _sectionRepository.GetById(id);

            var result = _mapper.Map<SectionWithDepartmentDto>(section);

            return result;
        }

        public async Task<SectionDto> UpdateSection(Guid id, SectionRequest dto)
        {
            var section = _mapper.Map<Section>(dto);

            var updatedSection = await _sectionRepository.Update(id, section);

            var responseDto = _mapper.Map<SectionDto>(updatedSection);

            return responseDto;
        }

        public async Task<bool> DeleteSection(Guid id)
        {
            return await _sectionRepository.Delete(id);
        }
        
        public async Task<SectionAndDepartmentDto> GetSectionsAndDepartment(Guid officeId)
        {
            return await _sectionRepository.GetSectionsAndDepartment(officeId);
        }
    }
}
