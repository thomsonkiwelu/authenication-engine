using conservation_backend.Features.Users;
using conservation_backend.Shared;

namespace conservation_backend.Features.DistrictContexts;

public record DistrictContextPaginationDto : PaginationDto
{
    public string? DistrictProfileId { get; set; }
  
    public string? FieldName { get; set; }
}

public record DistrictContextDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; } = string.Empty;
    public Guid DistrictProfileId { get; set; }
    public string FieldName { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public string CreatedBy { get; set; } = string.Empty;
    public DateTime? UpdatedAt { get; set; }
    public string UpdatedBy { get; set; } = string.Empty;
    //Relationship
    public UserMinimalDto Creator { get; set; } = new UserMinimalDto();
    public UserMinimalDto Updater { get; set; } = new UserMinimalDto();
}

public record DistrictContextRequestDto
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; } = string.Empty;
    public Guid DistrictProfileId { get; set; }
    public string FieldName { get; set; } = string.Empty;
    public Guid CreatedBy { get; set; }
    public Guid? UpdatedBy { get; set; }
}

public record DistrictContextResponseDto : DistrictContextDto
{
    public int RowNumber { get; set; }
}

public record DevelopmentOrganizationDto
{
    public Guid Id { get; set; }
    public Guid DistrictProfileId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string TimeOfOperation { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string TelephoneNumber { get; set; } = string.Empty;
    public string ContactPersonName { get; set; } = string.Empty;
    public string ContactPersonMobile { get; set; } = string.Empty;
    public string AreaOfOperation { get; set; } = string.Empty;
    public string TypeOfOperation { get; set; } = string.Empty;
    public string Comment { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public string CreatedBy { get; set; } = string.Empty;
    public DateTime? UpdatedAt { get; set; }
    public string UpdatedBy { get; set; } = string.Empty;
}

public record DevelopmentOrganizationRequestDto
{
    public string Name { get; set; } = string.Empty;
    public string TimeOfOperation { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string TelephoneNumber { get; set; } = string.Empty;
    public string ContactPersonName { get; set; } = string.Empty;
    public string ContactPersonMobile { get; set; } = string.Empty;
    public string AreaOfOperation { get; set; } = string.Empty;
    public string TypeOfOperation { get; set; } = string.Empty;
    public string Comment { get; set; } = string.Empty;
    public Guid DistrictProfileId { get; set; }
    public Guid CreatedBy { get; set; }
    public Guid? UpdatedBy { get; set; }
}