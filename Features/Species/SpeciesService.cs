using conservation_backend.Features.Species.Interfaces;
using conservation_backend.Shared;
using MapsterMapper;

namespace conservation_backend.Features.Species
{
    public class SpeciesService(ISpeciesRepository repository, IMapper mapper): ISpeciesService
    {
        private readonly ISpeciesRepository _speciesRepository = repository;

        private readonly IMapper _mapper = mapper;

        public async Task<PagedList<SpeciesResponseDto>> GetAllSpeciesData(SpeciesPaginationDto dto)
        {
            var pagedData = await _speciesRepository.GetPagedData(dto);

            var startIndex = (pagedData.Page - 1) * pagedData.PageSize + 1;

            var dtoList = _mapper.Map<List<SpeciesResponseDto>>(pagedData.Data)
                .Select((dto, index) => dto with { RowNumber = startIndex + index }).ToList();

            return new PagedList<SpeciesResponseDto>(
                items: dtoList,
                page: pagedData.Page,
                pageSize: pagedData.PageSize,
                totalCount: pagedData.TotalCount
            );
        }

        public async Task<SpeciesDto> CreateSpecies(SpeciesRequest dto)
        {
            var species = _mapper.Map<Species>(dto);

            var createdSpecies = await _speciesRepository.Create(species);

            var responseDto = _mapper.Map<SpeciesDto>(createdSpecies);

            return responseDto;
        }   

        public async Task<SpeciesDto> GetSpeciesById(Guid id)
        {
            var species = await _speciesRepository.GetById(id);

            var result = _mapper.Map<SpeciesDto>(species);

            return result;
        }

        public async Task<SpeciesDto> UpdateSpecies(Guid id, SpeciesRequest dto)
        {
            var species = _mapper.Map<Species>(dto);

            var updatedSpecies = await _speciesRepository.Update(id, species);

            var responseDto = _mapper.Map<SpeciesDto>(updatedSpecies);

            return responseDto;
        }

        public async Task<bool> DeleteSpecies(Guid id)
        {
            return await _speciesRepository.Delete(id);
        }
    }
}
