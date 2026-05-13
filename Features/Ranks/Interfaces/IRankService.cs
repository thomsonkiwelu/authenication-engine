using authentication_engine.Shared;

namespace authentication_engine.Features.Ranks.Interfaces
{
    public interface IRankService
    {
        Task<PagedList<RankResponseDto>> GetAllRanksData(PaginationDto dto);

        Task<RankDto> CreateRank(RankRequest dto);

        Task<RankDto> GetRankById(Guid id);

        Task<RankDto> UpdateRank(Guid id, RankRequest dto);

        Task<bool> DeleteRank(Guid id);
    }
}
