using conservation_backend.Features.Files.Interfaces;
using MapsterMapper;

namespace conservation_backend.Features.Files;

public class FileService(
    IFileRepository repository,
    IHttpContextAccessor httpContext,
    IWebHostEnvironment environment,
    IMapper mapper
    ): IFileService
{
    private readonly IFileRepository _fileRepository = repository;
    private readonly IHttpContextAccessor _httpContext = httpContext;
    private readonly IWebHostEnvironment _environment = environment;
    private readonly IMapper _mapper = mapper;

    public async Task<FileDto> AddFile(IFormFile file)
    {
        ValidateFile(file);

        var entity = await UploadFile(file);
        
        await _fileRepository.Create(entity);
        
        return await GetFileById(entity.Id);
    }

    public async Task<FileEntity> UploadFile(IFormFile file)
    {
        var extension = Path.GetExtension(file.FileName).ToLower();
        
        string folder = extension == ".pdf"
            ? "uploads/documents"
            : "uploads/images";
        
        var webRoot = _environment.WebRootPath ?? Path.Combine(_environment.ContentRootPath, "wwwroot");
        var uploadPath = Path.Combine(webRoot, folder);

        if (!Directory.Exists(uploadPath))
            Directory.CreateDirectory(uploadPath);
        
        var fileName = $"{Guid.NewGuid()}{extension}";
        var filePath = Path.Combine(uploadPath, fileName);
        
        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await file.CopyToAsync(stream);
        }

        var entity = new FileEntity
        {
            FileName = fileName,
            FilePath = Path.Combine(folder, fileName).Replace("\\", "/"),
            FileType = file.ContentType,
            FileSize = file.Length
        };

        return entity;
    }
    
    public void ValidateFile(IFormFile file)
    {
        if (file == null || file.Length == 0)
            throw new BadHttpRequestException("File cannot be empty",StatusCodes.Status400BadRequest);

        if (file.Length > 5 * 1024 * 1024)
            throw new BadHttpRequestException("File size exceeds 5MB",StatusCodes.Status400BadRequest);

        var extension = Path.GetExtension(file.FileName).ToLower();

        var allowedExtensions = new[]
        {
            ".jpg", ".jpeg", ".png", ".webp", ".pdf"
        };

        if (!allowedExtensions.Contains(extension))
            throw new BadHttpRequestException("Invalid file type",StatusCodes.Status400BadRequest);

        var allowedContentTypes = new[]
        {
            "image/jpeg",
            "image/png",
            "image/webp",
            "application/pdf"
        };

        if (!allowedContentTypes.Contains(file.ContentType))
            throw new BadHttpRequestException("Invalid file content type",StatusCodes.Status400BadRequest);
    }

    public async Task<FileDto> GetFileById(Guid fileId)
    {
        var file = await _fileRepository.GetById(fileId);
        var result = _mapper.Map<FileDto>(file);
        
        var request = _httpContext.HttpContext!.Request;
        var baseUrl = $"{request.Scheme}://{request.Host}";
        
        result.FileUrl = $"{baseUrl}/{file.FilePath}";
        return result;
    }
    
    public async Task<FileDto> UpdateUploadedFile(string fileId, string entityName, Guid entityId)
    {
        var file = await _fileRepository.GetById(Guid.Parse(fileId));
        
        file.EntityId = entityId;
        file.EntityName = entityName;
        
        var updated = await _fileRepository.Update(Guid.Parse(fileId), file);
        
        return await GetFileById(file.Id);
    }
    
    public async Task<List<FileDto>> GetFileByEntityData(Guid entityId, string entityName)
    {
        var files = await _fileRepository.GetFilesByEntityData(entityId, entityName);
        var results = _mapper.Map<List<FileDto>>(files);
     
        var request = _httpContext.HttpContext!.Request;
        var baseUrl = $"{request.Scheme}://{request.Host}";
        
        foreach (var file in results)
        {
            file.FileUrl = $"{baseUrl}/{file.FilePath}";
        }
        
        return results;
    }
    
    public async Task<FileDto> UpdateFile(UpdateFileDto dto)
    {
        if (!string.IsNullOrWhiteSpace(dto.Id))
            await _fileRepository.Delete(Guid.Parse(dto.Id));
        
        var entity = await UploadFile(dto.File);
        
        entity.EntityId = dto.EntityId;
        entity.EntityName = dto.EntityName;
        
        var created = await _fileRepository.Create(entity);
        
        return await GetFileById(created.Id);
    }
    
    public async Task<FileDto?> GetSingleFileByEntityData(Guid entityId, string entityName)
    {
        var file = await _fileRepository.GetSingleFileByEntityData(entityId, entityName);
        if (file is null) return null;
        
        var result = _mapper.Map<FileDto>(file);
        
        var request = _httpContext.HttpContext!.Request;
        var baseUrl = $"{request.Scheme}://{request.Host}";
        
        result.FileUrl = $"{baseUrl}/{file.FilePath}";
        return result;
    }
    
}