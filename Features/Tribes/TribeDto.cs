using conservation_backend.Features.Users;

namespace conservation_backend.Features.Tribes;

public record TribeDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public Guid CreatedBy { get; set; }
    public DateTime UpdatedAt { get; set; }
    public Guid? UpdatedBy { get; set; }
    //Relationship
    public UserMinimalDto Creator { get; set; } = new UserMinimalDto();
}

public record TribeRequestDto
{
    public string Name { get; set; } = string.Empty;
}

public record TribeResponseDto: TribeDto
{
    public int RowNumber { get; set; }
}