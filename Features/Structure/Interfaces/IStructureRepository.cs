using conservation_backend.Shared;

namespace conservation_backend.Features.Structure.Interfaces
{
    public interface IStructureRepository
    {
        Task<PagedList<StructureEntity>> GetPagedData(PaginationDto dto);

        Task<StructureEntity> Create(StructureEntity structure);

        Task<StructureEntity> GetById(Guid id);

        Task<StructureEntity> Update(Guid id, StructureEntity structure);

        Task<bool> Delete(Guid id);
    }
}
