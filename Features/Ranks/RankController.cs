using authentication_engine.Features.Ranks.Interfaces;
using authentication_engine.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace authentication_engine.Features.Ranks
{
    [Authorize]
    [ApiController]
    [Route("api/ranks")]
    public class RankController(IRankService rankService) : ControllerBase
    {
        private readonly IRankService _rankService = rankService;

        [HttpGet]
        [ProducesResponseType(typeof(ResponseWithPagination<List<RankDto>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetRanks([FromQuery] PaginationDto dto)
        {
            var pagedResult = await _rankService.GetAllRanksData(dto);

            return Ok(ApiHttpResponse.Page(pagedResult));
        }

        [HttpPost]
        [ProducesResponseType(typeof(ResponseWithData<RankDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateRank([FromBody] RankRequest dto)
        {
            var createdRank = await _rankService.CreateRank(dto);

            return Ok(ApiHttpResponse.Data(createdRank));
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ResponseWithData<RankDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetRankById(Guid id)
        {
            var rank = await _rankService.GetRankById(id);

            return Ok(ApiHttpResponse.Data(rank));
        }

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(ResponseWithData<RankDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateRank(Guid id, [FromBody] RankRequest dto)
        {
            var rank = await _rankService.UpdateRank(id, dto);

            return Ok(ApiHttpResponse.Data(rank));
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(ResponseWithMessage), StatusCodes.Status200OK)]
        public async Task<IActionResult> DeleteRank(Guid id)
        {
            await _rankService.DeleteRank(id);

            return Ok(ApiHttpResponse.Message(ResponseMessages.Deleted));
        }
    }
}
