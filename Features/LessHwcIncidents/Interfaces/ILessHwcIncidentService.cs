using conservation_backend.Shared;

namespace conservation_backend.Features.LessHwcIncidents.Interfaces;

public interface ILessHwcIncidentService
{
    Task<LessHwcIncidentResponseDto> GetById(Guid id);

    Task<LessHwcIncidentResponseDto> Create(LessHwcIncidentCreateRequest request);

    Task<LessHwcIncidentResponseDto> Update(Guid id, LessHwcIncidentUpdateRequest request);

    Task<bool> Delete(Guid id);

    Task<PagedList<LessHwcIncidentHeaderDto>> GetHeaders(LessHwcIncidentHeadersRequest request);
}
