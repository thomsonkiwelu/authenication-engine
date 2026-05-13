using System.ComponentModel.DataAnnotations;
using conservation_backend.Features.LessRangerDivisionConfig;
using conservation_backend.Features.LessRangerStations;
using conservation_backend.Features.Offices;
using conservation_backend.Features.Parks;
using conservation_backend.Features.Staffs;
using conservation_backend.Shared;

namespace conservation_backend.Features.LessRangerDailyDivisions;

public class LessRangerDailyDivision : BaseEntity
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public Guid? ParkId { get; set; }

    public Park? Park { get; set; }

    public Guid? OfficeId { get; set; }

    public Office? Office { get; set; }

    public Guid LessRangerStationId { get; set; }

    public LessRangerStation LessRangerStation { get; set; } = null!;

    public DateOnly DutyDate { get; set; }

    [MaxLength(20)]
    public string Category { get; set; } = string.Empty; // officer | ranger
}

public class LessRangerDailyDivisionAssignment : BaseEntity
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public Guid LessRangerDailyDivisionId { get; set; }

    public LessRangerDailyDivision LessRangerDailyDivision { get; set; } = null!;

    public Guid DutyFieldDefinitionId { get; set; }

    public LessRangerDutyFieldDefinition DutyFieldDefinition { get; set; } = null!;

    public Guid StaffId { get; set; }

    public Staff Staff { get; set; } = null!;
}
