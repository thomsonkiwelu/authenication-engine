using conservation_backend.Features.Species.Interfaces;
using conservation_backend.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace conservation_backend.Features.Species
{
    [Authorize]
    [ApiController]
    [Route("api/species")]
    public class SpeciesController(ISpeciesService speciesService): ControllerBase
    {
        private readonly ISpeciesService _speciesService = speciesService;

        [HttpGet]
        [ProducesResponseType(typeof(ResponseWithPagination<List<SpeciesDto>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetSpecies(
            [FromQuery] SpeciesPaginationDto dto
        )
        {
            var pagedResult = await _speciesService.GetAllSpeciesData(dto);

            return Ok(ApiHttpResponse.Page(pagedResult));
        }

        [HttpPost]
        [ProducesResponseType(typeof(ResponseWithData<SpeciesRequest>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateSpecies([FromBody] SpeciesRequest dto)
        {
            var createdSpecies = await _speciesService.CreateSpecies(dto);

            return Ok(ApiHttpResponse.Data(createdSpecies));
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ResponseWithData<SpeciesDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetSpeciesById(Guid id)
        {
            var species = await _speciesService.GetSpeciesById(id);

            return Ok(ApiHttpResponse.Data(species));
        }

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(ResponseWithData<SpeciesRequest>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateSpecies(Guid id, [FromBody] SpeciesRequest dto)
        {
            var species = await _speciesService.UpdateSpecies(id, dto);

            return Ok(ApiHttpResponse.Data(species));
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(ResponseWithMessage), StatusCodes.Status200OK)]
        public async Task<IActionResult> DeleteSpecies(Guid id)
        {
            await _speciesService.DeleteSpecies(id);

            return Ok(ApiHttpResponse.Message(ResponseMessages.Deleted));
        }
    }
}
