using conservation_backend.Features.Departments;
using conservation_backend.Shared;

namespace conservation_backend.Features.Sections
{
    public record SectionPaginationDto : PaginationDto
    {
        public string? DepartmentId { get; init; }
        
        public string? OfficeId { get; init; }
    }

    public record SectionRequest(
        string Name,
        string Code,
        Guid DepartmentId,
        Guid OfficeId
    );
    
    public record SectionResponseDto
    {
        public Guid Id { get; init; }
        
        public int RowNumber { get; init; }
        
        public string Name { get; init; } = string.Empty;
        
        public string Code { get; init; } = string.Empty;
        
        public Guid DepartmentId { get; init; }
        
        public Guid OfficeId { get; init; }
        
        public DateTime CreatedAt { get; init; }
        
        public string CreatedBy { get; init; } = string.Empty;
        
        public DepartmentDto? Department { get; init; }
    }

    public record SectionDto
    {
        public Guid Id { get; init; }
        
        public string Name { get; init; } = string.Empty;
        
        public string Code { get; init; } = string.Empty;
        
        public Guid DepartmentId { get; init; }
        
        public Guid OfficeId { get; init; }
        
        public DateTime CreatedAt { get; init; }
    }

    public record SectionWithDepartmentDto : SectionDto
    {
        public DepartmentDto? Department { get; init; }
    }
    
    public record SectionAndDepartmentDto
    {
        public List<OptionItemFormat> Departments { get; set; } = new();
        public List<SectionResponseDto> Sections { get; set; } = new List<SectionResponseDto>();
    }
}
