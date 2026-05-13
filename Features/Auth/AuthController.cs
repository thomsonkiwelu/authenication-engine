using conservation_backend.Features.Auth.Interfaces;
using conservation_backend.Features.Users;
using conservation_backend.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace conservation_backend.Features.Auth
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController(IAuthService authService) : ControllerBase
    {
        private readonly IAuthService _authService = authService;

        [HttpPost("login")]
        [ProducesResponseType(typeof(ResponseWithData<AuthResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseWithMessage), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Login([FromBody] LoginRequest dto)
        {
            var result = await _authService.Login(dto);

            return Ok(ApiHttpResponse.Data(result));
        }

        [Authorize]
        [HttpGet("me")]
        [ProducesResponseType(typeof(ResponseWithData<UserWithAccessControlDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseWithMessage), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetCurrentUser()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userId is null)
                throw new UnauthorizedAccessException(ResponseMessages.Unauthorized);

            var result = await _authService.GetCurrentUser(Guid.Parse(userId));
            
            return Ok(ApiHttpResponse.Data(result));
        }
    }
}
