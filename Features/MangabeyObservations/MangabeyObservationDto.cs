using conservation_backend.Features.Parks;
using conservation_backend.Features.Species;
using conservation_backend.Features.Users;
using conservation_backend.Shared;

namespace conservation_backend.Features.MangabeyObservations;

public record MangabeyObservationPaginationDto : PaginationDto
{
    public string? ParkId { get; set; }
}

public record MangabeyObservationDto
{
    public Guid Id { get; set; }
    public Guid ParkId { get; set; }
    public float NumberOfParticipant { get; set; }
    public string ActivityType { get; set; } = string.Empty;
    public string? Coordinates { get; set; } = string.Empty;
    public bool? IsGroupTypeLetu { get; set; }
    public bool? IsGroupTypeLingine { get; set; }
    public DateTime CreatedAt { get; set; }
    public string CreatedBy { get; set; } = string.Empty;
    public DateTime? UpdatedAt { get; set; }
    public string UpdatedBy { get; set; } = string.Empty;
    //Relationship
    public ParkDto Park { get; set; } = new ParkDto();
    public UserMinimalDto Creator { get; set; } = new UserMinimalDto();
}

public record MangabeyMatingBehaviorDto
{
    public Guid Id { get; set; }
    public string MaleMating { get; set; } = string.Empty;
    public string FemaleMating { get; set; } = string.Empty;
    public string WhatHappened { get; set; } = string.Empty;
    public Guid MangabeyObservationId { get; set; }
    public DateTime CreatedAt { get; set; }
    public string CreatedBy { get; set; } = string.Empty;
    public DateTime? UpdatedAt { get; set; }
    public string UpdatedBy { get; set; } = string.Empty;
}

public record MangabeyFightingBehaviorDto
{
    public Guid Id { get; set; }
    public string AggressiveIndividual { get; set; } = string.Empty;
    public string AttackedIndividual { get; set; } = string.Empty;
    public string WhatHappened { get; set; } = string.Empty;
    public Guid MangabeyObservationId { get; set; }
    public DateTime CreatedAt { get; set; }
    public string CreatedBy { get; set; } = string.Empty;
    public DateTime? UpdatedAt { get; set; }
    public string UpdatedBy { get; set; } = string.Empty;
}

public record MangabeyEatingBehaviorDto
{
    public Guid Id { get; set; }
    public Guid SpeciesId { get; set; }
    public string ConsumedTreePart { get; set; } = string.Empty;
    public string OtherFood { get; set; } = string.Empty;
    public string? OtherInsect { get; set; } = string.Empty;
    public Guid MangabeyObservationId { get; set; }
    public DateTime CreatedAt { get; set; }
    public string CreatedBy { get; set; } = string.Empty;
    public DateTime? UpdatedAt { get; set; }
    public string UpdatedBy { get; set; } = string.Empty;
    //Relationship
    public SpeciesDto Species { get; set; } = new SpeciesDto();
}

public record MangabeyOtherSpecieObservationDto
{
    public Guid Id { get; set; }
    public Guid SpeciesId { get; set; }
    public string ActivityObserved { get; set; } = string.Empty;
    public string NumberOfAnimalSeen { get; set; } = string.Empty;
    public string MangabeyBehavior { get; set; } = string.Empty;
    public Guid MangabeyObservationId { get; set; }
    public DateTime CreatedAt { get; set; }
    public string CreatedBy { get; set; } = string.Empty;
    public DateTime? UpdatedAt { get; set; }
    public string UpdatedBy { get; set; } = string.Empty;
    //Relationship
    public SpeciesDto Species { get; set; } = new SpeciesDto();
}

public record MangabeyObservationRequestDto
{
    public Guid ParkId { get; set; }
    public float NumberOfParticipant { get; set; }
    public string ActivityType { get; set; } = string.Empty;
    public string? Coordinates { get; set; } = string.Empty;
    public bool? IsGroupTypeLetu { get; set; } = false;
    public bool? IsGroupTypeLingine { get; set; } = false;
    //MangabeyEatingBehaviorRequestDto
    public string? SpeciesId { get; set; } = string.Empty;
    public string? ConsumedTreePart { get; set; } = string.Empty;
    public string? OtherFood { get; set; } = string.Empty;
    public string? OtherInsect { get; set; } = string.Empty;
    public Guid CreatedBy { get; set; }
    public Guid? UpdatedBy { get; set; }
    public List<MangabeyMatingBehaviorRequestDto> MatingData { get; set; } = new List<MangabeyMatingBehaviorRequestDto>();
    public List<MangabeyFightingBehaviorRequestDto> FightingData { get; set; } = new List<MangabeyFightingBehaviorRequestDto>();
    public List<MangabeyOtherSpecieObservationRequestDto> OtherAnimalData { get; set; } = new List<MangabeyOtherSpecieObservationRequestDto>();
}

public record MangabeyMatingBehaviorRequestDto
{
    public string MaleMating { get; set; } = string.Empty;
    public string FemaleMating { get; set; } = string.Empty;
    public string WhatHappened { get; set; } = string.Empty;
}

public record MangabeyFightingBehaviorRequestDto
{
    public string AggressiveIndividual { get; set; } = string.Empty;
    public string AttackedIndividual { get; set; } = string.Empty;
    public string WhatHappened { get; set; } = string.Empty;
}

public record MangabeyOtherSpecieObservationRequestDto
{
    public Guid SpeciesId { get; set; }
    public string ActivityObserved { get; set; } = string.Empty;
    public string NumberOfAnimalSeen { get; set; } = string.Empty;
    public string MangabeyBehavior { get; set; } = string.Empty;
}

public record GetMangabeyObservationDto : MangabeyObservationDto
{
    public MangabeyEatingBehaviorDto MangabeyEatingBehavior { get; set; } = new MangabeyEatingBehaviorDto();
    public List<MangabeyMatingBehaviorDto> MangabeyMatingBehaviors { get; set; } = new List<MangabeyMatingBehaviorDto>();
    public List<MangabeyFightingBehaviorDto> MangabeyFightingBehaviors { get; set; } = new List<MangabeyFightingBehaviorDto>();
    public List<MangabeyOtherSpecieObservationDto> MangabeyOtherSpecieObservations { get; set; } = new List<MangabeyOtherSpecieObservationDto>();
    public UserMinimalDto CreatedByUser { get; set; } = new UserMinimalDto();
    public UserMinimalDto UpdatedByUser { get; set; } = new UserMinimalDto();
}

public record MangabeyObservationResponseDto : MangabeyObservationDto
{
    public int RowNumber { get; set; }
}