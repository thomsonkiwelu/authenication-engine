using System.Security.Claims;
using conservation_backend.Config;

namespace conservation_backend.Shared;

public interface IUserContext
{
    Guid GetUserId();

    List<Guid> GetAuthorizedParkIds(AppDBContext context);
}