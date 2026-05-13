using System.Security.Claims;
using authentication_engine.Config;

namespace authentication_engine.Shared;

public class UserContext(IHttpContextAccessor httpContextAccessor) : IUserContext
{
    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;
    
    public Guid GetUserId()
    {
        var userId = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (Guid.TryParse(userId, out var guid))
            return guid;

        throw new UnauthorizedAccessException("User ID not found in token.");
    }

    public List<Guid> GetAuthorizedParkIds(AppDBContext context)
    {
        var userId = GetUserId();
        
        var assignedParkIds = context.UserParks
            .Where(up => up.UserId == userId)
            .Select(up => up.ParkId)
            .ToList();
        
        return assignedParkIds;
    }
    
}