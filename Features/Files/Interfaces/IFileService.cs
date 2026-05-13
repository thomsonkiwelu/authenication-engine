namespace conservation_backend.Features.Files.Interfaces;

public interface IFileService
{
    Task<FileDto> AddFile(IFormFile file);
    
    Task<FileEntity> UploadFile(IFormFile file);
    
    void ValidateFile(IFormFile file);
    
    Task<FileDto> GetFileById(Guid fileId);
    
    Task<FileDto> UpdateUploadedFile(string fileId, string entityName, Guid entityId);
    
    Task<List<FileDto>> GetFileByEntityData(Guid entityId, string entityName);

    Task<FileDto> UpdateFile(UpdateFileDto dto);
    
    Task<FileDto?> GetSingleFileByEntityData(Guid entityId, string entityName);
}