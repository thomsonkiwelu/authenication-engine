using authentication_engine.Shared;

namespace authentication_engine.Features.Parks;

public record ParkRequest(
    string Name,
    string? Code,
    string Zone
);

public record ParkPaginationDto : PaginationDto
{
    public string? Zone { get; init; }
}

public record ParkDto
{
    public Guid Id { get; set; }

    public string Name { get; set; } = string.Empty;
    
    public string Code { get; set; } = string.Empty;
    
    public string Zone { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; }
    
    public string CreatedBy { get; set; }
}

public record ParkResponseDto : ParkDto
{
    public int RowNumber { get; set; }
}