using conservation_backend.Features.LessRangerDivisionConfig.Interfaces;

namespace conservation_backend.Features.LessRangerDivisionConfig;

public class LessRangerDivisionConfigService(ILessRangerDivisionConfigRepository repository)
    : ILessRangerDivisionConfigService
{
    private readonly ILessRangerDivisionConfigRepository _repository = repository;

    public Task<LessRangerDivisionConfigResponseDto> GetConfig()
    {
        return _repository.GetConfig();
    }

    public Task<bool> UpdateConfig(LessRangerDivisionConfigUpdateRequest request)
    {
        return _repository.UpdateConfig(request);
    }
}
