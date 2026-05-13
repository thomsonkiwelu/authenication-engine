using System.ComponentModel.DataAnnotations;
using authentication_engine.Features.LessOperationalZones;
using authentication_engine.Features.LessRangerGroups;
using authentication_engine.Features.LessRangerStations;
using authentication_engine.Features.Offices;
using authentication_engine.Features.Parks;
using authentication_engine.Features.Staffs;
using authentication_engine.Shared.Entities;

namespace authentication_engine.Features.LessStaffPostings;

public class LessStaffPosting : BaseEntity
{
    public Guid Id { get; set; }

    public Guid StaffId { get; set; }

    public Staff Staff { get; set; } = null!;

    public Guid? ParkId { get; set; }

    public Park? Park { get; set; }

    public Guid? LessOperationalZoneId { get; set; }

    public LessOperationalZone? LessOperationalZone { get; set; }

    public Guid? OfficeId { get; set; }

    public Office? Office { get; set; }

    public Guid? LessRangerStationId { get; set; }

    public LessRangerStation? LessRangerStation { get; set; }

    public Guid? LessRangerGroupId { get; set; }

    public LessRangerGroup? LessRangerGroup { get; set; }

    public DateTime EffectiveFrom { get; set; } = DateTime.UtcNow;

    public DateTime? EffectiveTo { get; set; }

    [MaxLength(255)]
    public string? Remarks { get; set; }
}
