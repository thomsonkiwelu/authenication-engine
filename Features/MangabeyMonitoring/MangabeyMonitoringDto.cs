using conservation_backend.Features.Parks;
using conservation_backend.Features.Users;
using conservation_backend.Shared;

namespace conservation_backend.Features.MangabeyMonitoring;

public record MangabeyMonitoringPaginationDto : PaginationDto
{
    public string? ParkId { get; set; }
}

public record MangabeyMonitoringDto
{
    public Guid Id { get; set; }
    public Guid ParkId { get; set; }
    public int NumberOfParticipant { get; set; }
    public int NumberOfSpecies { get; set; }
    public string ActivityType { get; set; } = string.Empty;
    public string? Coordinates { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public string CreatedBy { get; set; } = string.Empty;
    public DateTime? UpdatedAt { get; set; }
    public string UpdatedBy { get; set; } = string.Empty;
    //Relationship
    public ParkDto Park { get; set; } = new ParkDto();
    public UserMinimalDto Creator { get; set; } = new UserMinimalDto();
    public UserMinimalDto Updater { get; set; } = new UserMinimalDto();
}

public record MangabeyMonitoringRequestDto
{
    public Guid ParkId { get; set; }
    public int NumberOfParticipant { get; set; }
    public int? NumberOfSpecies { get; set; }
    public string ActivityType { get; set; } = string.Empty;
    public string? Coordinates { get; set; } = string.Empty;
    public Guid CreatedBy { get; set; }
    public Guid? UpdatedBy { get; set; }
}

public record MangabeyMonitoringResponseDto : MangabeyMonitoringDto
{
    public int RowNumber { get; set; }
}