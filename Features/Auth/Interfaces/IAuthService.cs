
using authentication_engine.Features.Users;

namespace authentication_engine.Features.Auth.Interfaces
{
    public interface IAuthService
    {
        Task<AuthResponse> Login(LoginRequest dto);

        Task<UserWithAccessControlDto> GetCurrentUser(Guid userId);
    }
}
