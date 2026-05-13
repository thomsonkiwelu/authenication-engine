using System.ComponentModel.DataAnnotations;
using conservation_backend.Features.LessOperationalZones;
using conservation_backend.Features.LessRangerGroups;
using conservation_backend.Features.LessRangerStations;
using conservation_backend.Features.Offices;
using conservation_backend.Features.Parks;
using conservation_backend.Features.Staffs;
using conservation_backend.Shared;

namespace conservation_backend.Features.LessStaffPostings;

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
