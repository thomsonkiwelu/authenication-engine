using conservation_backend.Features.Users;

namespace conservation_backend.Features.Auth
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
