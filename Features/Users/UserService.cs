using authentication_engine.Features.Users.Interfaces;
using authentication_engine.Shared;
using MapsterMapper;

namespace authentication_engine.Features.Users;

public class UserService(IUserRepository repository, IMapper mapper) : IUserService
{
    private readonly IUserRepository _userRepository = repository;

    private readonly IMapper _mapper = mapper;
    
    public async Task<PagedList<UserResponseDto>> GetAllUsersData(PaginationDto dto)
    {
        var pagedData = await _userRepository.GetPagedData(dto);
        var startIndex = (pagedData.Page - 1) * pagedData.PageSize + 1;
        var dtoList = _mapper.Map<List<UserResponseDto>>(pagedData.Data)
            .Select((dto, index) => dto with { RowNumber = startIndex + index }).ToList();

        return new PagedList<UserResponseDto>(
            items: dtoList,
            page: pagedData.Page,
            pageSize: pagedData.PageSize,
            totalCount: pagedData.TotalCount
        );
    }

    public Task<UserDto> CreateUser(UserDto dto)
    {
        throw new NotImplementedException();
    }

    public async Task<UserDetailsDto> GetUserById(Guid id)
    {
        return await _userRepository.GetById(id);
    }

    public Task<UserDto> UpdateUser(Guid id, UserDto dto)
    {
        throw new NotImplementedException();
    }

    public Task<bool> DeleteUser(Guid id)
    {
        throw new NotImplementedException();
    }
}