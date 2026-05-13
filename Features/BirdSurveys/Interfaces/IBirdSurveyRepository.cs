using conservation_backend.Features.BirdSurveys;
using conservation_backend.Shared;

namespace conservation_backend.Features.BirdSurveys.Interfaces;

public interface IBirdSurveyRepository
{
    Task<PagedList<BirdSurveyResponseDto>> GetPagedData(BirdSurveyPaginationDto dto);
    
    Task<string> Create(BirdSurveyRequestDto dto);

    Task<GetBirdSurveyDto> GetById(Guid id);

    Task<string> Update(Guid id, BirdSurveyRequestDto dto);

    Task<bool> Delete(Guid id);
}