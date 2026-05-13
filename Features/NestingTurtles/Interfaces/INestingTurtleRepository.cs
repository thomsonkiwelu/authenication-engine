using conservation_backend.Shared;

namespace conservation_backend.Features.NestingTurtles.Interfaces;

public interface INestingTurtleRepository
{
    Task<PagedList<NestingTurtle>> GetPagedData(NestingTurtlePaginationDto dto);

    Task<NestingTurtle> Create(NestingTurtle nestingTurtle);

    Task<NestingTurtle> GetById(Guid id);

    Task<NestingTurtle> Update(Guid id, NestingTurtle nestingTurtle);

    Task<bool> Delete(Guid id);
}