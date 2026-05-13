using authentication_engine.Features.Ranks;
using authentication_engine.Shared;

namespace authentication_engine.Features.Staffs;

public record StaffPaginationDto : PaginationDto
{
    public string? RankId { get; init; }
}

public record StaffRequest(
    string FirstName,
    string LastName,
    string Email,
    string PhoneNumber,
    string Status,
    Guid RankId
);

public record StaffDto
{
    public Guid Id { get; set; }
        
    public string FirstName { get; set; } = string.Empty;
        
    public string LastName { get; set; } = string.Empty;
    
    public string Email { get; set; } = string.Empty;
    
    public string PhoneNumber { get; set; } = string.Empty;
    
    public string Status { get; set; } = string.Empty;
    
    public Guid RankId { get; set; }
        
    public DateTime CreatedAt { get; set; }
    
    public Guid? CreatedBy { get; set; }
    
    //Relatinship
    public RankDto Rank { get; set; } = new RankDto();
}

public record StaffResponseDto : StaffDto
{
    public int RowNumber { get; set; }
}

public record DepartmentStaffDto
{
    public Guid Id { get; init; }
    
    public int RowNumber { get; init; }
    
    public string FirstName { get; init; } = string.Empty;
        
    public string LastName { get; init; } = string.Empty;
    
    public string Email { get; init; } = string.Empty;
    
    public string PhoneNumber { get; init; } = string.Empty;
    
    public string ModelType { get; init; } = string.Empty;
    
    public string AssignedTo { get; init; } = string.Empty;
    
    public DateTime CreatedAt { get; init; }
}

public record OrganizationContextDto
{
    public List<OptionItemFormat> Units { get; set; } = new();
    public List<OptionItemFormat> Sections { get; set; } = new();
    public List<OptionItemFormat> Departments { get; set; } = new();
    public List<OptionItemFormat> Staffs { get; set; } = new();
    public List<OptionItemFormat> Roles { get; set; } = new();
    public List<DepartmentStaffDto> AssignedStaffs { get; set; } = new List<DepartmentStaffDto>();
}