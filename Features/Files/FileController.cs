using conservation_backend.Features.Files.Interfaces;
using conservation_backend.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace conservation_backend.Features.Files;

[Authorize]
[ApiController]
[Route("api/files")]

public class FileController(IFileService fileService): ControllerBase
{
    private readonly IFileService _fileService = fileService;
    
    [HttpPost("upload")]
    [ProducesResponseType(typeof(ResponseWithData<FileDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Upload([FromForm] UploadFileDto dto)
    {
        var result = await _fileService.AddFile(dto.File);

        return Ok(ApiHttpResponse.Data(result,"File uploaded successfully"));
    }
    
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ResponseWithData<FileDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await _fileService.GetFileById(id);

        return Ok(ApiHttpResponse.Data(result));
    }
    
    [HttpPost("update")]
    [ProducesResponseType(typeof(ResponseWithData<FileDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Upload([FromForm] UpdateFileDto dto)
    {
        var result = await _fileService.UpdateFile(dto);

        return Ok(ApiHttpResponse.Data(result,"File uploaded successfully"));
    }
    
}