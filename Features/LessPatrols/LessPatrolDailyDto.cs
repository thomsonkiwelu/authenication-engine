using conservation_backend.Shared;

namespace conservation_backend.Features.LessPatrols;

public record LessPatrolDailyResponseDto
{
    public Guid? PatrolDailyId { get; init; }

    public Guid? ParkId { get; init; }

    public Guid? OfficeId { get; init; }

    public Guid? LessOperationalZoneId { get; init; }

    public string? LessOperationalZoneName { get; init; }

    public Guid StationId { get; init; }

    public string StationName { get; init; } = string.Empty;

    public DateOnly DutyDate { get; init; }

    public int ManDaysPlanned { get; init; }

    public int ManDaysPerformed { get; init; }

    public int FootPatrolPlanned { get; init; }

    public int FootPatrolPerformed { get; init; }

    public int VehiclePatrolPlanned { get; init; }

    public int VehiclePatrolPerformed { get; init; }

    public int BoatPatrolPlanned { get; init; }

    public int BoatPatrolPerformed { get; init; }

    public int AirPatrolPlanned { get; init; }

    public int AirPatrolPerformed { get; init; }

    public decimal AirPatrolHoursPlanned { get; init; }

    public decimal AirPatrolHoursPerformed { get; init; }

    public decimal AreaInspectedKm { get; init; }
}

public record LessPatrolDailyGetRequest
{
    public string StationId { get; init; } = string.Empty;

    public string DutyDate { get; init; } = string.Empty; // yyyy-MM-dd
}

public record LessPatrolDailySaveRequest
{
    public string StationId { get; init; } = string.Empty;

    public string DutyDate { get; init; } = string.Empty; // yyyy-MM-dd

    public int ManDaysPlanned { get; init; }

    public int ManDaysPerformed { get; init; }

    public int FootPatrolPlanned { get; init; }

    public int FootPatrolPerformed { get; init; }

    public int VehiclePatrolPlanned { get; init; }

    public int VehiclePatrolPerformed { get; init; }

    public int BoatPatrolPlanned { get; init; }

    public int BoatPatrolPerformed { get; init; }

    public int AirPatrolPlanned { get; init; }

    public int AirPatrolPerformed { get; init; }

    public decimal AirPatrolHoursPlanned { get; init; }

    public decimal AirPatrolHoursPerformed { get; init; }

    public decimal AreaInspectedKm { get; init; }
}

public record LessPatrolDailyHeadersRequest : PaginationDto
{
    public string? FromDate { get; init; } // yyyy-MM-dd

    public string? ToDate { get; init; } // yyyy-MM-dd

    public string? LessOperationalZoneId { get; init; }

    public string? StationId { get; init; }
}

public record LessPatrolDailyHeaderDto
{
    public DateOnly DutyDate { get; init; }

    public Guid? LessOperationalZoneId { get; init; }

    public string? LessOperationalZoneName { get; init; }

    public Guid StationId { get; init; }

    public string StationName { get; init; } = string.Empty;

    public int ManDaysPlanned { get; init; }

    public int ManDaysPerformed { get; init; }

    public int FootPatrolPlanned { get; init; }

    public int FootPatrolPerformed { get; init; }

    public int VehiclePatrolPlanned { get; init; }

    public int VehiclePatrolPerformed { get; init; }

    public int BoatPatrolPlanned { get; init; }

    public int BoatPatrolPerformed { get; init; }

    public int AirPatrolPlanned { get; init; }

    public int AirPatrolPerformed { get; init; }

    public decimal AirPatrolHoursPlanned { get; init; }

    public decimal AirPatrolHoursPerformed { get; init; }

    public decimal AreaInspectedKm { get; init; }

    public DateTime CreatedAt { get; init; }

    public string Status { get; init; } = string.Empty;
}
