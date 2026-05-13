using conservation_backend.Features.Departments;
using conservation_backend.Features.Sections;
using conservation_backend.Shared;

namespace conservation_backend.Features.Units
{
    public record UnitPaginationDto : PaginationDto
    {
        public string? SectionId { get; init; }
        
        public string? OfficeId { get; init; }
        
        public string? DepartmentId { get; init; }
    }

    public record UnitRequest(
        string Name,
        string Code,
        Guid? DepartmentId,
        Guid? SectionId,
        Guid OfficeId
    );

    public record UnitDto
    {
        public Guid Id { get; init; }
        
        public string Name { get; init; } = string.Empty;
        
        public string Code { get; init; } = string.Empty;
        
        public Guid? DepartmentId { get; init; }
        
        public Guid? SectionId { get; init; }
        
        public DateTime CreatedAt { get; init; }
        
        public string CreatedBy { get; init; } = string.Empty;
    }
    
    public record UnitResponseDto : UnitDto
    {
        public int RowNumber { get; init; }
        
        public DepartmentDto? Department { get; init; }
        
        public SectionDto? Section { get; init; }
    }
    
    public record UnitsWithDepartmentAndSectionDto
    {
        public List<OptionItemFormat> Sections { get; set; } = new();
        public List<OptionItemFormat> Departments { get; set; } = new();
        public List<UnitResponseDto> Units { get; set; } = new List<UnitResponseDto>();
    }

}
