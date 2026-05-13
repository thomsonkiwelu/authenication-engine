using conservation_backend.Features.Offices.Interface;
using conservation_backend.Shared;
using MapsterMapper;

namespace conservation_backend.Features.Offices
{
    public class OfficeService(IOfficeRepository repository, IMapper mapper): IOfficeService
    {
        private readonly IOfficeRepository _officeRepository = repository;

        private readonly IMapper _mapper = mapper;

        public async Task<PagedList<OfficeResponseDto>> GetAllOfficesData(OfficePaginationDto dto)
        {
            var pagedData = await _officeRepository.GetPagedData(dto);

            var startIndex = (pagedData.Page - 1) * pagedData.PageSize + 1;

            var dtoList = _mapper.Map<List<OfficeResponseDto>>(pagedData.Data)
                .Select((dto, index) => dto with { RowNumber = startIndex + index }).ToList();

            return new PagedList<OfficeResponseDto>(
                items: dtoList,
                page: pagedData.Page,
                pageSize: pagedData.PageSize,
                totalCount: pagedData.TotalCount
            );
        }

        public async Task<OfficeDto> CreateOffice(OfficeRequest dto)
        {
            var office = _mapper.Map<Office>(dto);

            var createdOffice = await _officeRepository.Create(office);

            var responseDto = _mapper.Map<OfficeDto>(createdOffice);

            return responseDto;
        }

        public async Task<OfficeWithStructureDto> GetOfficeById(Guid id)
        {
            var office = await _officeRepository.GetById(id);

            var result = _mapper.Map<OfficeWithStructureDto>(office);

            return result;
        }

        public async Task<OfficeDto> UpdateOffice(Guid id, OfficeRequest dto)
        {
            var office = _mapper.Map<Office>(dto);

            var updatedOffice = await _officeRepository.Update(id, office);

            var responseDto = _mapper.Map<OfficeDto>(updatedOffice);

            return responseDto;
        }

        public async Task<bool> DeleteOffice(Guid id)
        {
            return await _officeRepository.Delete(id);
        }
    }
}
