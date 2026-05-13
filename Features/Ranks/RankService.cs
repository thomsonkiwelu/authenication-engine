using authentication_engine.Features.Ranks.Interfaces;
using authentication_engine.Shared;
using MapsterMapper;

namespace authentication_engine.Features.Ranks
{
    public class RankService(IRankRepository repository, IMapper mapper): IRankService
    {
        private readonly IRankRepository _rankRepository = repository;

        private readonly IMapper _mapper = mapper;

        public async Task<PagedList<RankResponseDto>> GetAllRanksData(PaginationDto dto)
        {
            var pagedData = await _rankRepository.GetPagedData(dto);

            var startIndex = (pagedData.Page - 1) * pagedData.PageSize + 1;

            var dtoList = _mapper.Map<List<RankResponseDto>>(pagedData.Data)
                .Select((dto, index) => dto with { RowNumber = startIndex + index }).ToList();

            return new PagedList<RankResponseDto>(
                items: dtoList,
                page: pagedData.Page,
                pageSize: pagedData.PageSize,
                totalCount: pagedData.TotalCount
            );
        }

        public async Task<RankDto> CreateRank(RankRequest dto)
        {
            var rank = _mapper.Map<Rank>(dto);

            var createdRank = await _rankRepository.Create(rank);

            var responseDto = _mapper.Map<RankDto>(createdRank);

            return responseDto;
        }

        public async Task<RankDto> GetRankById(Guid id)
        {
            var rank = await _rankRepository.GetById(id);

            var result = _mapper.Map<RankDto>(rank);

            return result;
        }

        public async Task<RankDto> UpdateRank(Guid id, RankRequest dto)
        {
            var rank = _mapper.Map<Rank>(dto);

            var updatedRank = await _rankRepository.Update(id, rank);

            var responseDto = _mapper.Map<RankDto>(updatedRank);

            return responseDto;
        }

        public async Task<bool> DeleteRank(Guid id)
        {
            return await _rankRepository.Delete(id);
        }
    }
}
