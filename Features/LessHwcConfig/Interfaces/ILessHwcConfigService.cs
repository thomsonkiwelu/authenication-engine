namespace conservation_backend.Features.LessHwcConfig.Interfaces;

public interface ILessHwcConfigService
{
    Task<LessHwcConfigResponseDto> GetConfig();

    Task<bool> UpdateConfig(LessHwcConfigUpdateRequest request);
}
