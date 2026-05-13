using conservation_backend.Features.Users;
using System.Security.Claims;
using conservation_backend.Features.Staffs;

namespace conservation_backend.Features.Auth.Interfaces
{
    public interface ITokenService
    {
        //string GenerateAccessToken(User user, List<string> roles);
        string GenerateAccessToken(User user, Staff staff);

        string GenerateRefreshToken();

        ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
    }
}
