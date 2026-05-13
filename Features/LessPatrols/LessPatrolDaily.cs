using System.ComponentModel.DataAnnotations;
using conservation_backend.Features.Offices;
using conservation_backend.Features.LessRangerStations;
using conservation_backend.Features.Parks;
using conservation_backend.Shared;
using Microsoft.EntityFrameworkCore;

namespace conservation_backend.Features.LessPatrols;

public class LessPatrolDaily : BaseEntity
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public Guid? ParkId { get; set; }

    public Park? Park { get; set; }

    public Guid? OfficeId { get; set; }

    public Office? Office { get; set; }

    public Guid LessRangerStationId { get; set; }

    public LessRangerStation LessRangerStation { get; set; } = null!;

    public DateOnly DutyDate { get; set; }

    public int ManDaysPlanned { get; set; }

    public int ManDaysPerformed { get; set; }

    public int FootPatrolPlanned { get; set; }

    public int FootPatrolPerformed { get; set; }

    public int VehiclePatrolPlanned { get; set; }

    public int VehiclePatrolPerformed { get; set; }

    public int BoatPatrolPlanned { get; set; }

    public int BoatPatrolPerformed { get; set; }

    public int AirPatrolPlanned { get; set; }

    public int AirPatrolPerformed { get; set; }

    [Precision(12, 2)]
    public decimal AirPatrolHoursPlanned { get; set; }

    [Precision(12, 2)]
    public decimal AirPatrolHoursPerformed { get; set; }

    [Precision(12, 2)]
    public decimal AreaInspectedKm { get; set; }
}
