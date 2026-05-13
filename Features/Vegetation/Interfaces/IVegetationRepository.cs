using conservation_backend.Shared;

namespace conservation_backend.Features.Vegetation.Interfaces
{
    public interface IVegetationRepository
    {
        Task<PagedList<VegetationResponseDto>> GetPagedData(VegetationPaginationDto dto);

        Task<string> Create(VegetationRequestDto dto);

        Task<GetVegetationResponseDto> GetById(Guid id);

        Task<string> Update(Guid id, VegetationRequestDto dto);

        Task<bool> Delete(Guid id);
    }
}
