using conservation_backend.Shared;

namespace conservation_backend.Features.Wards.Interfaces;

public interface IWardService
{
    Task<PagedList<WardResponseDto>> GetAllWards(WardPaginationDto dto);

    Task<WardDto> CreateWard(WardRequest dto);

    Task<WardDto> GetWardById(Guid id);

    Task<WardDto> UpdateWard(Guid id, WardRequest dto);

    Task<bool> DeleteWard(Guid id);
}