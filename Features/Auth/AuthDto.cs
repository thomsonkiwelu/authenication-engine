using authentication_engine.Features.Users;

namespace authentication_engine.Features.Auth
{
    public record LoginRequest(
        string Username,
        string Password
    );

    public record AuthResponse
    {
        public string AccessToken { get; init; } = string.Empty;

        //public string RefreshToken { get; init; } = string.Empty;

        public UserDto? User { get; set; }
    }
}
