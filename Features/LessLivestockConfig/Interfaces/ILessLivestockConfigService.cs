namespace conservation_backend.Features.LessLivestockConfig.Interfaces;

public interface ILessLivestockConfigService
{
    Task<LessLivestockConfigResponseDto> GetConfig();

    Task<bool> UpdateConfig(LessLivestockConfigUpdateRequest request);
}
