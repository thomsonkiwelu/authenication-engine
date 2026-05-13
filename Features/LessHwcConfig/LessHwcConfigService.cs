using conservation_backend.Features.LessHwcConfig.Interfaces;

namespace conservation_backend.Features.LessHwcConfig;

public class LessHwcConfigService(ILessHwcConfigRepository repository) : ILessHwcConfigService
{
    private readonly ILessHwcConfigRepository _repository = repository;

    public Task<LessHwcConfigResponseDto> GetConfig()
    {
        return _repository.GetConfig();
    }

    public Task<bool> UpdateConfig(LessHwcConfigUpdateRequest request)
    {
        return _repository.UpdateConfig(request);
    }
}
