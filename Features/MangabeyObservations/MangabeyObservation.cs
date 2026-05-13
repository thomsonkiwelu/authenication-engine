using System.ComponentModel.DataAnnotations;
using conservation_backend.Features.Parks;
using conservation_backend.Shared;

namespace conservation_backend.Features.MangabeyObservations;

public class MangabeyObservation: BaseEntity
{
    public Guid Id { get; set; }
    
    public Guid ParkId { get; set; }
    public Park Park { get; set; } = null!;
    
    [MaxLength(50)]
    public string ActivityType { get; set; } = string.Empty;
    
    public float NumberOfParticipant { get; set; }
    
    [MaxLength(250)]
    public string? Coordinates { get; set; } = string.Empty;
    
    public bool? IsGroupTypeLetu { get; set; }
    
    public bool? IsGroupTypeLingine { get; set; }
}

public class MangabeyMatingBehavior: BaseEntity
{
    public Guid Id { get; set; }
    
    [MaxLength(50)]
    public string MaleMating { get; set; } = string.Empty;
    
    [MaxLength(50)]
    public string FemaleMating { get; set; } = string.Empty;
    
    [MaxLength(50)]
    public string WhatHappened { get; set; } = string.Empty;
        
    public Guid MangabeyObservationId { get; set; }
    public MangabeyObservation MangabeyObservation { get; set; } = null!;
}

public class MangabeyFightingBehavior: BaseEntity
{
    public Guid Id { get; set; }
    
    [MaxLength(50)]
    public string AggressiveIndividual { get; set; } = string.Empty;
    
    [MaxLength(50)]
    public string AttackedIndividual { get; set; } = string.Empty;
    
    [MaxLength(50)]
    public string WhatHappened { get; set; } = string.Empty;
        
    public Guid MangabeyObservationId { get; set; }
    public MangabeyObservation MangabeyObservation { get; set; } = null!;
}

public class MangabeyOtherSpecieObservation: BaseEntity
{
    public Guid Id { get; set; }
    
    public Guid SpeciesId { get; set; }
    public Species.Species Species { get; set; } = null!;
    
    [MaxLength(50)]
    public string ActivityObserved { get; set; } = string.Empty;
    
    [MaxLength(50)]
    public string NumberOfAnimalSeen { get; set; } = string.Empty;
    
    [MaxLength(50)]
    public string MangabeyBehavior { get; set; } = string.Empty;
        
    public Guid MangabeyObservationId { get; set; }
    public MangabeyObservation MangabeyObservation { get; set; } = null!;
}

public class MangabeyEatingBehavior: BaseEntity
{
    public Guid Id { get; set; }
    
    public Guid SpeciesId { get; set; }
    public Species.Species Species { get; set; } = null!;
    
    [MaxLength(50)]
    public string ConsumedTreePart { get; set; } = string.Empty;
    
    [MaxLength(50)]
    public string OtherFood { get; set; } = string.Empty;
    
    [MaxLength(50)]
    public string? OtherInsect { get; set; } = string.Empty;
        
    public Guid MangabeyObservationId { get; set; }
    public MangabeyObservation MangabeyObservation { get; set; } = null!;
}