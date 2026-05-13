using conservation_backend.Shared;

namespace conservation_backend.Features.BirdSurveys.Interfaces;

public interface IBirdSurveyService
{
    Task<PagedList<BirdSurveyResponseDto>> GetPagedBirdSurveys(BirdSurveyPaginationDto dto);
    
    Task<GetBirdSurveyDto> CreateBirdSurvey(BirdSurveyRequestDto dto);
    
    Task<GetBirdSurveyDto> GetBirdSurveyById(Guid id);

    Task<GetBirdSurveyDto> UpdateBirdSurvey(Guid id , BirdSurveyRequestDto dto);

    Task<bool> DeleteBirdSurvey(Guid id);
}