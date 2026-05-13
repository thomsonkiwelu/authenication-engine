namespace conservation_backend.Features.Files.Interfaces;

public interface IFileRepository
{
    Task<FileEntity> Create(FileEntity file);
    
    Task<FileEntity> GetById(Guid id);
    
    Task<FileEntity> Update(Guid id, FileEntity file);
    
    Task<List<FileEntity>> GetFilesByEntityData(Guid entityId, string entityName);
    
    Task<bool> Delete(Guid id);
    
    Task<FileEntity?> GetSingleFileByEntityData(Guid entityId, string entityName);
    
}