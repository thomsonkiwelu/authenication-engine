using authentication_engine.Features.LessRangerStations;
using authentication_engine.Features.Users;
using authentication_engine.Shared;

namespace authentication_engine.Features.LessRangerGroups;

public record LessRangerGroupPaginationDto : PaginationDto
{
    public string? LessRangerStationId { get; init; }
}

public record LessRangerGroupRequest(
    string Name,
    string LessRangerStationId,
    string? Code
);

public record LessRangerGroupDto
{
    public Guid Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string? Code { get; set; }

    public Guid LessRangerStationId { get; set; }

    public LessRangerStationDto LessRangerStation { get; set; } = new LessRangerStationDto();

    public DateTime CreatedAt { get; set; }

    public string CreatedBy { get; set; } = string.Empty;

    public DateTime? UpdatedAt { get; set; }

    public string UpdatedBy { get; set; } = string.Empty;
}

public record LessRangerGroupResponseDto : LessRangerGroupDto
{
    public int RowNumber { get; set; }

    public UserMinimalDto Creator { get; set; } = new UserMinimalDto();
}
