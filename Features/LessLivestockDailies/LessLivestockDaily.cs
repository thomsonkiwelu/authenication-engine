using System.ComponentModel.DataAnnotations;
using conservation_backend.Features.LessRangerStations;
using conservation_backend.Features.Offices;
using conservation_backend.Features.Parks;
using conservation_backend.Shared;
using Microsoft.EntityFrameworkCore;

namespace conservation_backend.Features.LessLivestockDailies;

public class LessLivestockDaily : BaseEntity
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public Guid? ParkId { get; set; }

    public Park? Park { get; set; }

    public Guid? OfficeId { get; set; }

    public Office? Office { get; set; }

    public Guid LessRangerStationId { get; set; }

    public LessRangerStation LessRangerStation { get; set; } = null!;

    public DateOnly DutyDate { get; set; }

    public ICollection<LessLivestockDailyLivestock> Livestock { get; set; } = new List<LessLivestockDailyLivestock>();

    public ICollection<LessLivestockDailyAction> Actions { get; set; } = new List<LessLivestockDailyAction>();
}

public class LessLivestockDailyLivestock : BaseEntity
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public Guid LessLivestockDailyId { get; set; }

    public LessLivestockDaily LessLivestockDaily { get; set; } = null!;

    [MaxLength(50)]
    public string LivestockTypeKey { get; set; } = string.Empty;

    public int Count { get; set; }
}

public class LessLivestockDailyAction : BaseEntity
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public Guid LessLivestockDailyId { get; set; }

    public LessLivestockDaily LessLivestockDaily { get; set; } = null!;

    [MaxLength(50)]
    public string LivestockTypeKey { get; set; } = string.Empty;

    [MaxLength(50)]
    public string ActionOptionKey { get; set; } = string.Empty;

    [MaxLength(2000)]
    public string Description { get; set; } = string.Empty;

    [Precision(12, 2)]
    public decimal Amount { get; set; }

    [MaxLength(255)]
    public string ControlNumber { get; set; } = string.Empty;

    [MaxLength(255)]
    public string CaseNumber { get; set; } = string.Empty;
}
