using authentication_engine.Features.Offices;
using authentication_engine.Features.Structure.Interfaces;
using authentication_engine.Shared;
using MapsterMapper;

namespace authentication_engine.Features.Structure
{
    public class StructureService(IStructureRepository repository, IMapper mapper) : IStructureService
    {
        private readonly IStructureRepository _structureRepository = repository;

        private readonly IMapper _mapper = mapper;

        public async Task<PagedList<StructureResponseDto>> GetAllStructuresData(PaginationDto dto)
        {
            var pagedData = await _structureRepository.GetPagedData(dto);

            var startIndex = (pagedData.Page - 1) * pagedData.PageSize + 1;

            var dtoList = _mapper.Map<List<StructureResponseDto>>(pagedData.Data)
                .Select((dto, index) => dto with { RowNumber = startIndex + index }).ToList();

            return new PagedList<StructureResponseDto>(
                items: dtoList,
                page: pagedData.Page,
                pageSize: pagedData.PageSize,
                totalCount: pagedData.TotalCount
            );
        }

        public async Task<StructureDto> CreateStructure(StructureRequest dto)
        {
            var structure = _mapper.Map<StructureEntity>(dto);

            var createdStructure = await _structureRepository.Create(structure);

            var responseDto = _mapper.Map<StructureDto>(createdStructure);

            return responseDto;
        }

        public async Task<StructureWithOfficeDto> GetStructureById(Guid id)
        {
            var structure = await _structureRepository.GetById(id);

            var result = new StructureWithOfficeDto
            {
                Id = structure.Id,
                Name = structure.Name,
                Level = structure.Level,
                CreatedAt = structure.CreatedAt,
                Offices = structure.Offices.
                    OrderBy(office => office.Name)
                    .Select(office => new OfficeDto
                {
                    Id = office.Id,
                    Name = office.Name,
                    Code = office.Code,
                    ParentOffice = office.ParentOffice,
                    HeadOfOffice = office.HeadOfOffice,
                    CreatedAt = office.CreatedAt
                }).ToList()
            };

            return result;
        }

        public async Task<StructureDto> UpdateStructure(Guid id, StructureRequest dto)
        {
            var structure = _mapper.Map<StructureEntity>(dto);

            var updatedStructure = await _structureRepository.Update(id, structure);

            var responseDto = _mapper.Map<StructureDto>(updatedStructure);

            return responseDto;
        }

        public async Task<bool> DeleteStructure(Guid id)
        {
            return await _structureRepository.Delete(id);
        }

    }
}
