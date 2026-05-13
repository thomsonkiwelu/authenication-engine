using conservation_backend.Features.LessRangerDailyDivisions.Interfaces;
using conservation_backend.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace conservation_backend.Features.LessRangerDailyDivisions;

[Authorize]
[ApiController]
[Route("api/less-ranger-daily-divisions")]
public class LessRangerDailyDivisionController(ILessRangerDailyDivisionService service) : ControllerBase
{
    private readonly ILessRangerDailyDivisionService _service = service;

    [HttpGet]
    [ProducesResponseType(typeof(ResponseWithData<LessRangerDailyDivisionResponseDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Get([FromQuery] LessRangerDailyDivisionGetRequest request)
    {
        var result = await _service.GetDivision(request);
        return Ok(ApiHttpResponse.Data(result));
    }

    [HttpPut]
    [ProducesResponseType(typeof(ResponseWithData<LessRangerDailyDivisionResponseDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Save([FromBody] LessRangerDailyDivisionSaveRequest request)
    {
        var result = await _service.SaveDivision(request);
        return Ok(ApiHttpResponse.Data(result));
    }

    [HttpGet("headers")]
    [ProducesResponseType(typeof(ResponseWithPagination<List<LessRangerDailyDivisionHeaderDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetHeaders([FromQuery] LessRangerDailyDivisionHeadersRequest request)
    {
        var result = await _service.GetHeaders(request);
        return Ok(ApiHttpResponse.Page(result));
    }

    [HttpGet("reports/per-field")]
    [ProducesResponseType(typeof(ResponseWithPagination<List<LessRangerDailyDivisionPerFieldReportRowDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetPerFieldReport([FromQuery] LessRangerDailyDivisionPerFieldReportRequest request)
    {
        var result = await _service.GetPerFieldReport(request);
        return Ok(ApiHttpResponse.Page(result));
    }

    [HttpGet("reports/per-station")]
    [ProducesResponseType(typeof(ResponseWithPagination<List<LessRangerDailyDivisionPerStationReportRowDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetPerStationReport([FromQuery] LessRangerDailyDivisionPerStationReportRequest request)
    {
        var result = await _service.GetPerStationReport(request);
        return Ok(ApiHttpResponse.Page(result));
    }

    [HttpGet("reports/category-summary")]
    [ProducesResponseType(typeof(ResponseWithPagination<List<LessRangerDailyDivisionCategorySummaryReportRowDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetCategorySummaryReport([FromQuery] LessRangerDailyDivisionCategorySummaryReportRequest request)
    {
        var result = await _service.GetCategorySummaryReport(request);
        return Ok(ApiHttpResponse.Page(result));
    }
}
