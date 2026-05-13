using conservation_backend.Shared;

namespace conservation_backend.Features.Offices.Interface
{
    public interface IOfficeService
    {
        Task<PagedList<OfficeResponseDto>> GetAllOfficesData(OfficePaginationDto dto);

        Task<OfficeDto> CreateOffice(OfficeRequest dto);

        Task<OfficeWithStructureDto> GetOfficeById(Guid id);

        Task<OfficeDto> UpdateOffice(Guid id, OfficeRequest dto);

        Task<bool> DeleteOffice(Guid id);
    }
}
