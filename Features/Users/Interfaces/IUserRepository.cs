using authentication_engine.Features.Auth;
using authentication_engine.Shared;

namespace authentication_engine.Features.Users.Interfaces
{
    public interface IUserRepository
    {
        Task<PagedList<User>> GetPagedData(PaginationDto dto);

        Task<User> Create(User office);

        Task<UserDetailsDto> GetById(Guid id);

        Task<User> Update(Guid id, User office);

        Task<bool> Delete(Guid id);

        Task<User?> GetUserByUsername(LoginRequest dto);

        Task<UserWithAccessControlDto?> GetUserByIdWithAccessControl(Guid Id);
    }
}
