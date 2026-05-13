using conservation_backend.Features.BirdSurveys.Interfaces;
using conservation_backend.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace conservation_backend.Features.BirdSurveys;

[Authorize]
[ApiController]
[Route("api/bird-surveys")]
public class BirdSurveyController(IBirdSurveyService birdSurveyService) : ControllerBase
{
    private readonly IBirdSurveyService _birdSurveyService = birdSurveyService;
    
    [HttpGet]
    [ProducesResponseType(typeof(ResponseWithPagination<List<BirdSurveyResponseDto>>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetAllData([FromQuery] BirdSurveyPaginationDto dto)
    {
        var pagedResult = await _birdSurveyService.GetPagedBirdSurveys(dto);

        return Ok(ApiHttpResponse.Page(pagedResult));
    }
    
    [HttpPost]
    [ProducesResponseType(typeof(ResponseWithData<GetBirdSurveyDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] BirdSurveyRequestDto dto)
    {
        var result = await _birdSurveyService.CreateBirdSurvey(dto);

        return Ok(ApiHttpResponse.Data(result));
    }
    
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ResponseWithData<GetBirdSurveyDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await _birdSurveyService.GetBirdSurveyById(id);

        return Ok(ApiHttpResponse.Data(result));
    }
    
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(ResponseWithData<GetBirdSurveyDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Update(Guid id, [FromBody] BirdSurveyRequestDto dto)
    {
        var result = await _birdSurveyService.UpdateBirdSurvey(id, dto);

        return Ok(ApiHttpResponse.Data(result));
    }
    
    [HttpDelete("{id}")]
    [ProducesResponseType(typeof(ResponseWithMessage), StatusCodes.Status200OK)]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _birdSurveyService.DeleteBirdSurvey(id);

        return Ok(ApiHttpResponse.Message(ResponseMessages.Deleted));
    }
}