using authentication_engine.Features.Offices;
using authentication_engine.Features.Parks;
using authentication_engine.Features.Permissions;
using authentication_engine.Features.Roles;
using authentication_engine.Features.Staffs;
using authentication_engine.Features.SystemModules;
using authentication_engine.Shared;

namespace authentication_engine.Features.Users
{
    public record ParkMinimalDto
    {
        public Guid Id { get; init; }
        public string Name { get; init; } = string.Empty;
        public string Code { get; init; } = string.Empty;
        public string Zone { get; init; } = string.Empty;
    }

    public record UserDto
    {
        public Guid Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        //Relationship
        public StaffDto Staff { get; set; } = new StaffDto();
    }

    public record UserWithAccessControlDto : UserDto
    {
        public List<SystemModuleDto> Modules { get; set; } = new List<SystemModuleDto>();

        public List<RoleDto> Roles { get; set; } = new List<RoleDto>();

        public List<PermissionMinimalDto> Permissions { get; set; } = new List<PermissionMinimalDto>();
        
        public OfficeDto Office { get; set; } = new OfficeDto();

        public ParkMinimalDto? AssignedPark { get; set; }

        public List<ParkMinimalDto> AccessibleParks { get; set; } = new List<ParkMinimalDto>();
    }

    public record UserResponseDto : UserDto
    {
        public int RowNumber { get; init; }
        
        public StaffDto? Staff { get; init; }
        
        public DateTime CreatedAt { get; init; }
        
        public string CreatedBy { get; init; } = string.Empty;
    }
    
    public record StaffHistoriesData
    {
        public Guid Id { get; init; }
        
        public int RowNumber { get; init; }
        
        public string ModelType { get; init; } = string.Empty;
        
        public Guid DepartmentId { get; init; }
        
        public Guid StaffId { get; init; }
        
        public DateTime CreatedAt { get; init; }
        
        public string CreatedBy { get; init; } = string.Empty;
        
        public Dictionary<string, string> Department { get; init; } = new();
        
        public Dictionary<string, string> User { get; init; } = new();
    }
    
    public record UserDetailsDto
    {
        public Dictionary<string, string> User { get; init; } = new();
            
        public List<OptionItemFormat> Departments { get; set; } = new();
        
        public List<OptionItemFormat> Sections { get; set; } = new();
        
        public List<OptionItemFormat> Units { get; set; } = new();
        
        public List<OptionItemFormat> Roles { get; set; } = new();
        
        public List<OptionItemFormat> Parks { get; set; } = new();
        
        public List<RoleResponseDto> AssignedRoles { get; init; } = new List<RoleResponseDto>();
        
        public List<StaffHistoriesData> StaffHistories { get; set; } = new List<StaffHistoriesData>();
        
        public List<ParkResponseDto> AssignedParks { get; init; } = new List<ParkResponseDto>();
    }
    
    public record UserMinimalDto
    {
        public Guid Id { get; init; }
        
        public string Username { get; init; } = string.Empty;
        
        public string Email { get; init; } = string.Empty;
    }
}
