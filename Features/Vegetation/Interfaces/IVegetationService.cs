using conservation_backend.Shared;

namespace conservation_backend.Features.Vegetation.Interfaces
{
    public interface IVegetationService
    {
        Task<PagedList<VegetationResponseDto>> GetPagedVegetations(VegetationPaginationDto dto);
    
        Task<GetVegetationResponseDto> CreateVegetation(VegetationRequestDto dto);
        
        Task<GetVegetationResponseDto> GetVegetationById(Guid id);

        Task<GetVegetationResponseDto> UpdateVegetation(Guid id , VegetationRequestDto dto);

        Task<bool> DeleteVegetation(Guid id);
    }

}
