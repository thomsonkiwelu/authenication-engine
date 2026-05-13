using conservation_backend.Config;
using conservation_backend.Features.Files.Interfaces;
using conservation_backend.Shared;
using Microsoft.EntityFrameworkCore;

namespace conservation_backend.Features.Files;

public class FileRepository(AppDBContext context, IWebHostEnvironment environment, IUserContext userContext): IFileRepository
{
    private readonly AppDBContext _context = context;
    private readonly IWebHostEnvironment _environment = environment;
    private readonly IUserContext _userContext = userContext;
    
    public async Task<FileEntity> Create(FileEntity file)
    {
        file.CreatedBy = _userContext.GetUserId();
        
        _context.Files.Add(file);
        await _context.SaveChangesAsync();

        return file;
    }

    public async Task<FileEntity> GetById(Guid id)
    {
        var result = await _context.Files.FindAsync(id);
        
        if (result is null)
            throw new KeyNotFoundException($"No file records found.");

        return result;
    }
    
    public async Task<FileEntity> Update(Guid id, FileEntity file)
    {
        var rows = await _context.Database.ExecuteSqlInterpolatedAsync($@"
                UPDATE ""Files""
                SET ""EntityId"" = {file.EntityId}, ""EntityName"" = {file.EntityName},""UpdatedBy"" = {_userContext.GetUserId()},
                ""UpdatedAt"" = {DateTime.UtcNow} WHERE ""Id"" = {id};
            ");
            
        if (rows == 0)
            throw new KeyNotFoundException("No file records found.");

        var updated = await _context.Files.FindAsync(id);
        return updated ?? throw new KeyNotFoundException("Failed to retrieve updated record");
    }
    
    public async Task<List<FileEntity>> GetFilesByEntityData(Guid entityId, string entityName)
    {
        return await _context.Files
            .AsNoTracking()
            .Where(up => up.EntityId == entityId)
            .Where(up => up.EntityName == entityName)
            .ToListAsync();
    }
    
    public async Task<bool> Delete(Guid id)
    {
        var file = await _context.Files.FindAsync(id);
        if (file is null)
            throw new KeyNotFoundException("No file records found");
        
        // Delete physical file using WebRootPath
        var filePath = Path.Combine(_environment.WebRootPath, file.FilePath);
    
        if (System.IO.File.Exists(filePath))
        {
            System.IO.File.Delete(filePath);
        }

        _context.Files.Remove(file);
        await _context.SaveChangesAsync();

        return true;
    }
    
    public async Task<FileEntity?> GetSingleFileByEntityData(Guid entityId, string entityName)
    {
        return await _context.Files
            .AsNoTracking()
            .Where(up => up.EntityId == entityId && up.EntityName == entityName)
            .FirstOrDefaultAsync();
    }
}