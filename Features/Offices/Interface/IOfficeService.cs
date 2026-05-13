using authentication_engine.Shared;

namespace authentication_engine.Features.Offices.Interface
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
