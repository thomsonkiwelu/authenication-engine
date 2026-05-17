
using authentication_engine.Features.Users;

namespace authentication_engine.Features.Auth.Interfaces
{
    public interface IAuthService
    {
        Task<AuthResponse> Login(LoginRequest dto);
        

        Task<ThirdPartyVerifyResponse> ThirdPartyVerify(ThirdPartyVerifyRequest dto);
    }
}
