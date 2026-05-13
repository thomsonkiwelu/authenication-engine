using authentication_engine.Shared;

namespace authentication_engine.Features.SystemApplications.Interfaces
{
    public interface ISystemApplicationService
    {
        Task<PagedList<SystemApplicationResponseDto>> GetAllSystemApplications(PaginationDto dto);

        Task<SystemApplicationDto> CreateSystemApplication(SystemApplicationRequestDto dto);

        Task<SystemApplicationDto> GetSystemApplicationById(Guid id);

        Task<SystemApplicationDto> UpdateSystemApplication(Guid id, SystemApplicationRequestDto dto);

        Task<bool> DeleteSystemApplication(Guid id);
    }
}
