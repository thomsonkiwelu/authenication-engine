namespace conservation_backend.Shared;

public static class AppEntities
{
    public const string WaterQuantityEntity = "WaterQuantity";
    public const string WaterQualityEntity = "WaterQuality";
    public const string FirePrescriptionEntity = "FirePrescription";
    public const string WildFireEntity = "WildFire";
    public const string FireBreakEntity = "FireBreak";
    public const string FireSeminarEntity = "FireSeminar";
    public const string SightingTurtleEntity = "SightingTurtle";
    public const string NestingTurtleEntity = "NestingTurtle";
    public const string DeathTurtleEntity = "DeathTurtle";
    public const string MigratoryBirdEntity = "MigratoryBird";
    public const string RareSpeciesOccurrenceEntity = "RareSpeciesOccurrence";
    public const string GroundCountSightingEntity = "GroundCountSighting";
    public const string VegetationEntity = "Vegetation";
    public const string InvasiveSpeciesEntity = "InvasiveSpecies";
    public const string RoadKillEntity = "RoadKill";
    public const string WeatherEntity = "Weather";
    public const string VillageProfileEntity = "VillageProfile";
    public const string DistrictProfileEntity = "DistrictProfile";
}

public record OptionItemFormat
{
    public Guid Id { get; set; }
    public string Name { get; set; }
}