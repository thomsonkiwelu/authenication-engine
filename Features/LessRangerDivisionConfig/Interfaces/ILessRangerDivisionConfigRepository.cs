namespace conservation_backend.Features.LessRangerDivisionConfig.Interfaces;

public interface ILessRangerDivisionConfigRepository
{
    Task<LessRangerDivisionConfigResponseDto> GetConfig();

    Task<bool> UpdateConfig(LessRangerDivisionConfigUpdateRequest request);
}
