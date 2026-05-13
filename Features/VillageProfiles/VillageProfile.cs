using System.ComponentModel.DataAnnotations;
using conservation_backend.Features.Parks;
using conservation_backend.Features.Villages;
using conservation_backend.Shared;

namespace conservation_backend.Features.VillageProfiles;

public class VillageProfile: BaseEntity
{
    public Guid Id { get; set; }
    
    public Guid VillageId { get; set; }
    public Village Village { get; set; } = null!;
    
    public Guid ParkId { get; set; }
    public Park Park { get; set; } = null!;
    
    public float VillageSize { get; set; }
    
    public float NumberOfHousehold { get; set; }
    
    public float NumberOfMale { get; set; }
    
    public float NumberOfFemale { get; set; }
    
    public float NumberOfCow { get; set; }
    
    public float NumberOfSheep { get; set; }
    
    public float NumberOfGoat { get; set; }
    
    public float NumberOfDog { get; set; }
    
    public float Population { get; set; }
    
    [MaxLength(50)]
    public string LandStatus { get; set; } = string.Empty;
}