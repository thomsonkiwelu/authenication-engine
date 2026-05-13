using conservation_backend.Shared;

namespace conservation_backend.Features.DeathTurtles.Interfaces;

public interface IDeathTurtleService
{
    Task<PagedList<DeathTurtleResponseDto>> GetAllDeathTurtles(DeathTurtlePaginationDto dto);

    Task<DeathTurtleDto> CreateDeathTurtle(DeathTurtleRequestDto dto);

    Task<DeathTurtleDto> GetDeathTurtleById(Guid id);

    Task<DeathTurtleDto> UpdateDeathTurtle(Guid id, DeathTurtleRequestDto dto);

    Task<bool> DeleteDeathTurtle(Guid id);
}