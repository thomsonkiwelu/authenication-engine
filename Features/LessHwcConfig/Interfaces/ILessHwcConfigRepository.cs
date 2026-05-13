namespace conservation_backend.Features.LessHwcConfig.Interfaces;

public interface ILessHwcConfigRepository
{
    Task<LessHwcConfigResponseDto> GetConfig();

    Task<bool> UpdateConfig(LessHwcConfigUpdateRequest request);
}
