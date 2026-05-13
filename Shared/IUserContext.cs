using authentication_engine.Config;

namespace authentication_engine.Shared;

public interface IUserContext
{
    Guid GetUserId();

    List<Guid> GetAuthorizedParkIds(AppDBContext context);
}