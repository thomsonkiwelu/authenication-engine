using authentication_engine.Shared;

namespace authentication_engine.Features.Structure.Interfaces
{
    public interface IStructureService
    {
        Task<PagedList<StructureResponseDto>> GetAllStructuresData(PaginationDto dto);

        Task<StructureDto> CreateStructure(StructureRequest dto);

        Task<StructureWithOfficeDto> GetStructureById(Guid id);

        Task<StructureDto> UpdateStructure(Guid id, StructureRequest dto);

        Task<bool> DeleteStructure(Guid id);
    }
}
