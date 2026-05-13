
using conservation_backend.Features.Users;

namespace conservation_backend.Features.Auth.Interfaces
{
    public interface IAuthService
    {
        Task<AuthResponse> Login(LoginRequest dto);

        Task<UserWithAccessControlDto> GetCurrentUser(Guid userId);
    }
}
