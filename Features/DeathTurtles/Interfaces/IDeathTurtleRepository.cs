using conservation_backend.Shared;

namespace conservation_backend.Features.DeathTurtles.Interfaces;

public interface IDeathTurtleRepository
{
    Task<PagedList<DeathTurtle>> GetPagedData(DeathTurtlePaginationDto dto);

    Task<DeathTurtle> Create(DeathTurtle deathTurtle);

    Task<DeathTurtle> GetById(Guid id);

    Task<DeathTurtle> Update(Guid id, DeathTurtle deathTurtle);

    Task<bool> Delete(Guid id);
}