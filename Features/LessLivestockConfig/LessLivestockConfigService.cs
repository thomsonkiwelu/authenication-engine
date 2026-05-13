using conservation_backend.Features.LessLivestockConfig.Interfaces;

namespace conservation_backend.Features.LessLivestockConfig;

public class LessLivestockConfigService(ILessLivestockConfigRepository repository) : ILessLivestockConfigService
{
    private readonly ILessLivestockConfigRepository _repository = repository;

    public Task<LessLivestockConfigResponseDto> GetConfig()
    {
        return _repository.GetConfig();
    }

    public Task<bool> UpdateConfig(LessLivestockConfigUpdateRequest request)
    {
        return _repository.UpdateConfig(request);
    }
}
