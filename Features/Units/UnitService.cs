using authentication_engine.Features.Units.Interfaces;
using authentication_engine.Shared;
using MapsterMapper;

namespace authentication_engine.Features.Units
{
    public class UnitService(IUnitRepository repository, IMapper mapper) : IUnitService
    {
        private readonly IUnitRepository _unitRepository = repository;
        private readonly IMapper _mapper = mapper;

        public async Task<PagedList<UnitResponseDto>> GetAllUnitsData(UnitPaginationDto dto)
        {
            var pagedData = await _unitRepository.GetPagedData(dto);

            var startIndex = (pagedData.Page - 1) * pagedData.PageSize + 1;

            var dtoList = _mapper.Map<List<UnitResponseDto>>(pagedData.Data)
                .Select((dto, index) => dto with { RowNumber = startIndex + index }).ToList();

            return new PagedList<UnitResponseDto>(
                items: dtoList,
                page: pagedData.Page,
                pageSize: pagedData.PageSize,
                totalCount: pagedData.TotalCount
            );
        }

        public async Task<UnitDto> CreateUnit(UnitRequest dto)
        {
            var unit = _mapper.Map<Unit>(dto);

            var createdUnit = await _unitRepository.Create(unit);

            var responseDto = _mapper.Map<UnitDto>(createdUnit);

            return responseDto;
        }

        public async Task<UnitDto> GetUnitById(Guid id)
        {
            var section = await _unitRepository.GetById(id);

            var result = _mapper.Map<UnitDto>(section);

            return result;
        }

        public async Task<UnitDto> UpdateUnit(Guid id, UnitRequest dto)
        {
            var unit = _mapper.Map<Unit>(dto);

            var updated = await _unitRepository.Update(id, unit);

            var responseDto = _mapper.Map<UnitDto>(updated);

            return responseDto;
        }

        public async Task<bool> DeleteUnit(Guid id)
        {
            return await _unitRepository.Delete(id);
        }
        
        public async Task<UnitsWithDepartmentAndSectionDto> GetUnitsWithDepartmentAndSection(Guid officeId)
        {
            return await _unitRepository.GetUnitsWithDepartmentAndSection(officeId);
        }
    }
}
