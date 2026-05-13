namespace conservation_backend.Features.LessLivestockConfig.Interfaces;

public interface ILessLivestockConfigRepository
{
    Task<LessLivestockConfigResponseDto> GetConfig();

    Task<bool> UpdateConfig(LessLivestockConfigUpdateRequest request);
}
