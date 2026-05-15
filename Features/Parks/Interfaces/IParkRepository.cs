using authentication_engine.Features.Users;
using authentication_engine.Shared;

namespace authentication_engine.Features.Parks.Interfaces;

public interface IParkRepository
{
    Task<PagedList<Park>> GetPagedData(ParkPaginationDto dto);

    Task<Park> Create(Park park);

    Task<Park> GetById(Guid id);

    Task<Park> Update(Guid id, Park park);

    Task<bool> Delete(Guid id);

    Task<bool> AssignParkToUser(UserPark userPark);

    Task<bool> UnassignParkToUser(UserPark userPark);
}