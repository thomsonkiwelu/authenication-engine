namespace conservation_backend.Features.LessRangerDivisionConfig.Interfaces;

public interface ILessRangerDivisionConfigService
{
    Task<LessRangerDivisionConfigResponseDto> GetConfig();

    Task<bool> UpdateConfig(LessRangerDivisionConfigUpdateRequest request);
}
