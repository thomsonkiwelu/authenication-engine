using conservation_backend.Shared;

namespace conservation_backend.Features.LessRangerDailyDivisions.Interfaces;

public interface ILessRangerDailyDivisionService
{
    Task<LessRangerDailyDivisionResponseDto> GetDivision(LessRangerDailyDivisionGetRequest request);

    Task<LessRangerDailyDivisionResponseDto> SaveDivision(LessRangerDailyDivisionSaveRequest request);

    Task<PagedList<LessRangerDailyDivisionHeaderDto>> GetHeaders(LessRangerDailyDivisionHeadersRequest request);

    Task<PagedList<LessRangerDailyDivisionPerFieldReportRowDto>> GetPerFieldReport(
        LessRangerDailyDivisionPerFieldReportRequest request
    );

    Task<PagedList<LessRangerDailyDivisionPerStationReportRowDto>> GetPerStationReport(
        LessRangerDailyDivisionPerStationReportRequest request
    );

    Task<PagedList<LessRangerDailyDivisionCategorySummaryReportRowDto>> GetCategorySummaryReport(
        LessRangerDailyDivisionCategorySummaryReportRequest request
    );
}
