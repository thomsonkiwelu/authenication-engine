using conservation_backend.Features.Auth;
using conservation_backend.Shared;

namespace conservation_backend.Features.Users.Interfaces
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
