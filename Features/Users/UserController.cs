using authentication_engine.Features.Users.Interfaces;
using authentication_engine.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace authentication_engine.Features.Users;

[Authorize]
[ApiController]
[Route("api/users")]
public class UserController(IUserService userService): ControllerBase
{
    private readonly IUserService _userService = userService;

    [HttpGet]
    [ProducesResponseType(typeof(ResponseWithPagination<List<UserResponseDto>>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetUsers([FromQuery] PaginationDto dto)
    {
        var pagedResult = await _userService.GetAllUsersData(dto);

        return Ok(ApiHttpResponse.Page(pagedResult));
    }
    
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ResponseWithData<UserDetailsDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetSectionById(Guid id)
    {
        var result = await _userService.GetUserById(id);

        return Ok(ApiHttpResponse.Data(result));
    }
    
}