using System.ComponentModel.DataAnnotations;
using conservation_backend.Features.Offices;
using conservation_backend.Features.Parks;
using conservation_backend.Shared;
using Microsoft.EntityFrameworkCore;

namespace conservation_backend.Features.LessHwcIncidents;

public class LessHwcIncident : BaseEntity
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public Guid ParkId { get; set; }

    public Park Park { get; set; } = null!;

    public Guid? OfficeId { get; set; }

    public Office? Office { get; set; }

    public DateOnly IncidentDate { get; set; }

    [MaxLength(255)]
    public string District { get; set; } = string.Empty;

    [MaxLength(255)]
    public string Ward { get; set; } = string.Empty;

    [MaxLength(500)]
    public string Villages { get; set; } = string.Empty;

    [MaxLength(100)]
    public string IncidentCategoryKey { get; set; } = string.Empty;

    [MaxLength(255)]
    public string ReferenceNo { get; set; } = string.Empty;

    [MaxLength(50)]
    public string Status { get; set; } = "Reported";

    public int TotalIncidents { get; set; }

    [Precision(12, 2)]
    public decimal EstimatedLossTzs { get; set; }

    [MaxLength(50000)]
    public string DataJson { get; set; } = "{}";
}
