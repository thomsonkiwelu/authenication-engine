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

        public UserDto? User { get; set; } = new UserDto();
    }
    
    public record ThirdPartyVerifyRequest
    {
        public string Username { get; init; } = string.Empty;
        public string Password { get; init; } = string.Empty;
        public string GrantType { get; init; } = string.Empty;
    }
    
    public record ThirdPartyVerifyResponse
    {
        public string Message { get; set; } = string.Empty;
        
        public UserWithAccessControlDto? Result { get; set; } = new UserWithAccessControlDto();
    }
}
