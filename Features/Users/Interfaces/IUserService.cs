using authentication_engine.Shared;

namespace authentication_engine.Features.Users.Interfaces;

public interface IUserService
{
    Task<PagedList<UserResponseDto>> GetAllUsersData(PaginationDto dto);

    Task<UserDto> CreateUser(UserDto dto);

    Task<UserDetailsDto> GetUserById(Guid id);

    Task<UserDto> UpdateUser(Guid id, UserDto dto);

    Task<bool> DeleteUser(Guid id);
}