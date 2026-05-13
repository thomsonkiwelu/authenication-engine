using conservation_backend.Features.BirdSurveys.Interfaces;
using conservation_backend.Features.Files.Interfaces;
using conservation_backend.Shared;
using MapsterMapper;

namespace conservation_backend.Features.BirdSurveys;

public class BirdSurveyService(IBirdSurveyRepository repository, IFileService fileService): IBirdSurveyService
{
    private readonly IBirdSurveyRepository _birdSurveyRepository = repository;
    private readonly IFileService _fileService = fileService;
    
    public async Task<PagedList<BirdSurveyResponseDto>> GetPagedBirdSurveys(BirdSurveyPaginationDto dto)
    {
        var pagedData = await _birdSurveyRepository.GetPagedData(dto);
        
        return new PagedList<BirdSurveyResponseDto>(
            items: pagedData.Data,
            page: pagedData.Page,
            pageSize: pagedData.PageSize,
            totalCount: pagedData.TotalCount
        );
    }

    public async Task<GetBirdSurveyDto> CreateBirdSurvey(BirdSurveyRequestDto dto)
    {
        var birdSurveyId = await _birdSurveyRepository.Create(dto);
        
        if (string.IsNullOrWhiteSpace(birdSurveyId))
            throw new ArgumentNullException("Failure to create bird survey data");
        
        return await _birdSurveyRepository.GetById(Guid.Parse(birdSurveyId));
    }

    public async Task<GetBirdSurveyDto> GetBirdSurveyById(Guid id)
    {
        var result = await _birdSurveyRepository.GetById(id);

        foreach (var migratoryBird in result.MigratoryBirds)
        {
            migratoryBird.File = await _fileService.GetSingleFileByEntityData(
                migratoryBird.Id, AppEntities.MigratoryBirdEntity
            );
        }

        return result;
    }

    public async Task<GetBirdSurveyDto> UpdateBirdSurvey(Guid id, BirdSurveyRequestDto dto)
    {
        var birdSurveyId =  await _birdSurveyRepository.Update(id, dto);
        
        if (string.IsNullOrWhiteSpace(birdSurveyId))
            throw new ArgumentNullException("Failure to update bird survey data");
        
        return await _birdSurveyRepository.GetById(Guid.Parse(birdSurveyId));
    }

    public async Task<bool> DeleteBirdSurvey(Guid id)
    {
        return await _birdSurveyRepository.Delete(id);
    }
}