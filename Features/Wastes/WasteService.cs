using conservation_backend.Features.Wastes.Interfaces;
using conservation_backend.Shared;
using MapsterMapper;

namespace conservation_backend.Features.Wastes
{
    public class WasteService(IWasteRepository wasteRepository, IMapper mapper): IWasteService
    {
        private readonly IWasteRepository _wasteRepository = wasteRepository;
        private readonly IMapper _mapper = mapper;
        
        public async Task<PagedList<WasteResponseDto>> GetAllWastesData(WastePaginationDto dto)
        {
            var pagedData = await _wasteRepository.GetPagedData(dto);
        
            return new PagedList<WasteResponseDto>(
                items: pagedData.Data,
                page: pagedData.Page,
                pageSize: pagedData.PageSize,
                totalCount: pagedData.TotalCount
            );
        }
        
        public async Task<GetWasteDto> CreateWaste(WasteRequestDto dto)
        {
            var wasteId = await _wasteRepository.Create(dto);
        
            if (string.IsNullOrWhiteSpace(wasteId))
                throw new ArgumentNullException("Failure to create waste data");
        
            return await _wasteRepository.GetById(Guid.Parse(wasteId));
        }
        
        public async Task<GetWasteDto> GetWasteById(Guid id)
        {
            return await _wasteRepository.GetById(id);
        }

        public async Task<GetWasteDto> UpdateWaste(Guid id, WasteRequestDto dto)
        {
            var wasteId = await _wasteRepository.Update(id, dto);
        
            if (string.IsNullOrWhiteSpace(wasteId))
                throw new ArgumentNullException("Failure to update waste data");
        
            return await _wasteRepository.GetById(Guid.Parse(wasteId));
        }

        public async Task<bool> DeleteWaste(Guid id)
        {
            return await _wasteRepository.Delete(id);
        }
    }
}
