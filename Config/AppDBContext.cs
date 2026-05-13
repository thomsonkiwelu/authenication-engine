using conservation_backend.Features.AerialCensuses;
using conservation_backend.Features.Auth;
using conservation_backend.Features.BirdSurveys;
using conservation_backend.Features.DeathTurtles;
using conservation_backend.Features.Departments;
using conservation_backend.Features.DistrictContexts;
using conservation_backend.Features.DistrictProfiles;
using conservation_backend.Features.Districts;
using conservation_backend.Features.Divisions;
using conservation_backend.Features.Files;
using conservation_backend.Features.FireBreaks;
using conservation_backend.Features.FirePrescriptions;
using conservation_backend.Features.FireSeminars;
using conservation_backend.Features.GovernmentLeaders;
using conservation_backend.Features.GroundCounts;
using conservation_backend.Features.HabitatManipulations;
using conservation_backend.Features.InvasiveSpecies;
using conservation_backend.Features.LessActivities;
using conservation_backend.Features.LessHwcConfig;
using conservation_backend.Features.LessHwcIncidents;
using conservation_backend.Features.LessLivestockConfig;
using conservation_backend.Features.LessLivestockDailies;
using conservation_backend.Features.Locations;
using conservation_backend.Features.LessOperationalZones;
using conservation_backend.Features.LessPatrols;
using conservation_backend.Features.LessRangerGroups;
using conservation_backend.Features.LessRangerStations;
using conservation_backend.Features.LessRangerDivisionConfig;
using conservation_backend.Features.LessRangerDailyDivisions;
using conservation_backend.Features.LessStaffPostings;
using conservation_backend.Features.LineTransects;
using conservation_backend.Features.MangabeyMonitoring;
using conservation_backend.Features.MangabeyObservations;
using conservation_backend.Features.NestingTurtles;
using conservation_backend.Features.Offices;
using conservation_backend.Features.Parks;
using conservation_backend.Features.Permissions;
using conservation_backend.Features.Ranks;
using conservation_backend.Features.RareEndangered;
using conservation_backend.Features.Regions;
using conservation_backend.Features.RoadKills;
using conservation_backend.Features.Roles;
using conservation_backend.Features.Sections;
using conservation_backend.Features.SightingTurtles;
using conservation_backend.Features.Species;
using conservation_backend.Features.Staffs;
using conservation_backend.Features.Stations;
using conservation_backend.Features.Structure;
using conservation_backend.Features.SystemModules;
using conservation_backend.Features.Tribes;
using conservation_backend.Features.Units;
using conservation_backend.Features.Users;
using conservation_backend.Features.Vegetation;
using conservation_backend.Features.VillageContexts;
using conservation_backend.Features.VillageProfiles;
using conservation_backend.Features.Villages;
using conservation_backend.Features.Wards;
using conservation_backend.Features.Wastes;
using conservation_backend.Features.WaterBodies;
using conservation_backend.Features.WaterQualities;
using conservation_backend.Features.WaterQuantities;
using conservation_backend.Features.Weathers;
using conservation_backend.Features.WildFires;
using conservation_backend.Shared;
using conservation_backend.Shared.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace conservation_backend.Config
{
    public class AppDBContext(DbContextOptions<AppDBContext> options) : DbContext(options)
    {
        // All Tables
        public DbSet<Waste> Wastes {  get; set; }

        public DbSet<Species> Species { get; set; }

        public DbSet<Role> Roles { get; set; }

        public DbSet<PermissionEntity> Permissions { get; set; }

        public DbSet<RolePermission> RolePermissions { get; set; }

        public DbSet<Rank> Ranks { get; set; }

        public DbSet<StructureEntity> Structures { get; set; }

        public DbSet<Office> Offices { get; set; }

        public DbSet<Department> Departments { get; set; }

        public DbSet<Section> Sections { get; set; }

        public DbSet<Unit> Units { get; set; }

        public DbSet<User> Users { get; set; }

        public DbSet<RefreshToken> RefreshTokens { get; set; }

        public DbSet<RoleUser> RoleUsers { get; set; }

        public DbSet<SystemModule> SystemModules { get; set; }
        
        public DbSet<Staff> Staffs { get; set; }
        
        public DbSet<DepartmentStaff> DepartmentStaffs { get; set; }
        
        public DbSet<DepartmentStaffHistory> DepartmentStaffHistories { get; set; }
        
        public DbSet<Park> Parks { get; set; }
        
        public DbSet<Location> Locations { get; set; }
        
        public DbSet<Vegetation> Vegetations { get; set; }
        
        public DbSet<Disturbance> Disturbances { get; set; }
        
        public DbSet<DistanceSample> DistanceSamples { get; set; }
        
        public DbSet<SpeciesOccurrence> SpeciesOccurrences { get; set; }
        
        public DbSet<LifeForm> LifeForms { get; set; }
        
        public DbSet<Invasive> InvasiveSpecies { get; set; }
        
        public DbSet<EcologySelection> EcologySelections { get; set; }
        
        public DbSet<RoadKill> RoadKills { get; set; }
        
        public DbSet<AnimalDemographic> AnimalDemographics { get; set; }
        
        public DbSet<Station> Stations { get; set; }
        
        public DbSet<Weather> Weather { get; set; }
        
        public DbSet<WasteMaterial> WasteMaterials { get; set; }
        
        public DbSet<UserPark> UserParks { get; set; }

        public DbSet<LessOperationalZone> LessOperationalZones { get; set; }

        public DbSet<LessRangerStation> LessRangerStations { get; set; }

        public DbSet<LessRangerGroup> LessRangerGroups { get; set; }

        public DbSet<LessRangerRankCategory> LessRangerRankCategories { get; set; }

        public DbSet<LessStaffPosting> LessStaffPostings { get; set; }

        public DbSet<LessRangerDailyDivision> LessRangerDailyDivisions { get; set; }

        public DbSet<LessRangerDailyDivisionAssignment> LessRangerDailyDivisionAssignments { get; set; }

        public DbSet<LessPatrolDaily> LessPatrolDailies { get; set; }
        
        public DbSet<HabitatManipulation> HabitatManipulations { get; set; }
        
        public DbSet<AerialCensus> AerialCensuses { get; set; }
        
        public DbSet<WaterBody> WaterBodies { get; set; }
        
        public DbSet<LessRangerDutyFieldDefinition> LessRangerDutyFieldDefinitions { get; set; }
        
        public DbSet<LessActivity> LessActivities { get; set; }

        public DbSet<LessLivestockType> LessLivestockTypes { get; set; }

        public DbSet<LessLivestockActionOption> LessLivestockActionOptions { get; set; }

        public DbSet<LessLivestockDaily> LessLivestockDailies { get; set; }

        public DbSet<LessLivestockDailyLivestock> LessLivestockDailyLivestock { get; set; }

        public DbSet<LessLivestockDailyAction> LessLivestockDailyActions { get; set; }

        public DbSet<LessHwcTabDefinition> LessHwcTabDefinitions { get; set; }

        public DbSet<LessHwcFieldDefinition> LessHwcFieldDefinitions { get; set; }

        public DbSet<LessHwcFieldOption> LessHwcFieldOptions { get; set; }

        public DbSet<LessHwcIncident> LessHwcIncidents { get; set; }
        
        public DbSet<WaterQuality> WaterQualities { get; set; }
        
        public DbSet<WaterQuantity> WaterQuantities { get; set; }
        
        public DbSet<FileEntity> Files { get; set; }
        
        public DbSet<FirePrescription> FirePrescriptions { get; set; }
        
        public DbSet<WildFire> WildFires { get; set; }
        
        public DbSet<FireBreak> FireBreaks { get; set; }
        
        public DbSet<FireSeminar> FireSeminars { get; set; }
        
        public DbSet<SightingTurtle> SightingTurtles { get; set; }
        
        public DbSet<NestingTurtle> NestingTurtles { get; set; }
        
        public DbSet<DeathTurtle> DeathTurtles { get; set; }
        
        public DbSet<LineTransect> LineTransects { get; set; }
        
        public DbSet<MigratoryBird> MigratoryBirds { get; set; }
        
        public DbSet<BirdSurvey> BirdSurveys { get; set; }
        
        public DbSet<Region> Regions { get; set; }
        
        public DbSet<District> Districts { get; set; }
        
        public DbSet<Division> Divisions { get; set; }
        
        public DbSet<Ward> Wards { get; set; }
        
        public DbSet<Village> Villages { get; set; }
        
        public DbSet<RareEndangeredSpecies> RareEndangeredSpecies { get; set; }
        
        public DbSet<RareSpeciesOccurrence> RareSpeciesOccurrences { get; set; }
        
        public DbSet<GroundCount> GroundCounts { get; set; }
        
        public DbSet<GroundCountSighting> GroundCountSightings { get; set; }
        
        public DbSet<MangabeyObservation> MangabeyObservations { get; set; }
        
        public DbSet<MangabeyEatingBehavior> MangabeyEatingBehaviors { get; set; }
        
        public DbSet<MangabeyMatingBehavior> MangabeyMatingBehaviors { get; set; }
        
        public DbSet<MangabeyFightingBehavior> MangabeyFightingBehaviors { get; set; }
        
        public DbSet<MangabeyOtherSpecieObservation> MangabeyOtherSpecieObservations { get; set; }
        
        public DbSet<MangabeyMonitoring> MangabeyMonitoring  { get; set; }
        
        public DbSet<VillageProfile> VillageProfiles  { get; set; }
        
        public DbSet<GovernmentLeader> GovernmentLeaders  { get; set; }
        
        public DbSet<VillageContext> VillageContexts { get; set; }
        
        public DbSet<CommunitySelection> CommunitySelections { get; set; }
        
        public DbSet<Tribe> Tribes { get; set; }
        
        public DbSet<DistrictProfile> DistrictProfiles { get; set; }
        
        public DbSet<DistrictContext> DistrictContexts { get; set; }
        
        public DbSet<DevelopmentOrganization> DevelopmentOrganizations { get; set; }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Enforce UTC for all DateTime properties
            var dateTimeConverter = new ValueConverter<DateTime, DateTime>(
                v => v.Kind == DateTimeKind.Utc ? v : v.ToUniversalTime(),
                v => DateTime.SpecifyKind(v, DateTimeKind.Utc));

            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                foreach (var property in entityType.GetProperties()
                             .Where(p => p.ClrType == typeof(DateTime)))
                {
                    property.SetValueConverter(dateTimeConverter);
                }
            }
            
            // Apply configuration to all entities that inherit from BaseEntity
            foreach (var entityType in modelBuilder.Model.GetEntityTypes()
                         .Where(e => typeof(BaseEntity).IsAssignableFrom(e.ClrType)))
            {
                // Configure Creator relationship
                modelBuilder.Entity(entityType.ClrType)
                    .HasOne("Creator")
                    .WithMany()
                    .HasForeignKey("CreatedBy")
                    .OnDelete(DeleteBehavior.Restrict)
                    .IsRequired(false);

                // Configure Updater relationship
                modelBuilder.Entity(entityType.ClrType)
                    .HasOne("Updater")
                    .WithMany()
                    .HasForeignKey("UpdatedBy")
                    .OnDelete(DeleteBehavior.Restrict)
                    .IsRequired(false);
            }

            //INFO: RolePermisson Table Constraints
            modelBuilder.Entity<RolePermission>(entity =>
            {
                entity.HasKey(rp => new { rp.RoleId, rp.PermissionId });

                entity.HasOne(rp => rp.Role)
                    .WithMany()
                    .HasForeignKey(rp => rp.RoleId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(rp => rp.Permission)
                    .WithMany()
                    .HasForeignKey(rp => rp.PermissionId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            //INFO: LESS Ranger Daily Division constraints
            modelBuilder.Entity<LessRangerDailyDivision>(entity =>
            {
                entity.HasIndex(x => new { x.LessRangerStationId, x.DutyDate, x.Category }).IsUnique();
            });

            modelBuilder.Entity<LessRangerDailyDivisionAssignment>(entity =>
            {
                entity.HasIndex(x => new { x.LessRangerDailyDivisionId, x.StaffId }).IsUnique();
            });

            //INFO: LESS Staff Posting constraints
            modelBuilder.Entity<LessStaffPosting>(entity =>
            {
                entity.HasIndex(x => x.StaffId)
                    .IsUnique()
                    .HasFilter("\"EffectiveTo\" IS NULL AND \"DeletedAt\" IS NULL");
            });

            //INFO: LESS Patrol Daily constraints
            modelBuilder.Entity<LessPatrolDaily>(entity =>
            {
                entity.HasIndex(x => new { x.LessRangerStationId, x.DutyDate }).IsUnique();
            });

            //INFO: LESS Livestock Daily constraints
            modelBuilder.Entity<LessLivestockDaily>(entity =>
            {
                entity.HasIndex(x => new { x.LessRangerStationId, x.DutyDate }).IsUnique();
            });

            //INFO: LESS HWC constraints
            modelBuilder.Entity<LessHwcTabDefinition>(entity =>
            {
                entity.HasIndex(x => x.Key).IsUnique();
            });

            modelBuilder.Entity<LessHwcFieldDefinition>(entity =>
            {
                entity.HasIndex(x => new { x.TabDefinitionId, x.Key }).IsUnique();
            });

            modelBuilder.Entity<LessHwcFieldOption>(entity =>
            {
                entity.HasIndex(x => new { x.FieldDefinitionId, x.Value }).IsUnique();
            });

            modelBuilder.Entity<LessHwcIncident>(entity =>
            {
                entity.Property(x => x.DataJson).HasColumnType("jsonb");
                entity.Property(x => x.Status).HasDefaultValue("Reported");
                entity.HasIndex(x => new { x.ParkId, x.IncidentDate });
                entity.HasIndex(x => new { x.OfficeId, x.IncidentDate });
            });

            //INFO: RoleUser Table Constraints
            modelBuilder.Entity<RoleUser>(entity =>
            {
                entity.HasKey(rp => new { rp.RoleId, rp.UserId });

                entity.HasOne(rp => rp.Role)
                    .WithMany()
                    .HasForeignKey(rp => rp.RoleId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(rp => rp.User)
                    .WithMany()
                    .HasForeignKey(rp => rp.UserId)
                    .OnDelete(DeleteBehavior.Cascade);
            });
            
            //INFO: DepartmentStaff Table Constraints
            modelBuilder.Entity<DepartmentStaff>(entity =>
            {
                entity.HasKey(ds => new { ds.DepartmentId, ds.StaffId });
                
                entity.HasOne(st => st.Staff)
                    .WithMany()
                    .HasForeignKey(sk => sk.StaffId)
                    .OnDelete(DeleteBehavior.Cascade);
            });
            
            //INFO: UserPark Table Constraints
            modelBuilder.Entity<UserPark>(entity =>
            {
                entity.HasKey(up => new { up.UserId, up.ParkId });

                entity.HasOne(u => u.User)
                    .WithMany()
                    .HasForeignKey(u => u.UserId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(p => p.Park)
                    .WithMany()
                    .HasForeignKey(p => p.ParkId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            //INFO: Apply soft delete filter to each entity
            modelBuilder.Entity<Role>().HasQueryFilter(x => x.DeletedAt == null);
            modelBuilder.Entity<Rank>().HasQueryFilter(x => x.DeletedAt == null);
            modelBuilder.Entity<StructureEntity>().HasQueryFilter(x => x.DeletedAt == null);
            modelBuilder.Entity<Office>().HasQueryFilter(x => x.DeletedAt == null);
            modelBuilder.Entity<Department>().HasQueryFilter(x => x.DeletedAt == null);
            modelBuilder.Entity<Section>().HasQueryFilter(x => x.DeletedAt == null);
            modelBuilder.Entity<Unit>().HasQueryFilter(x => x.DeletedAt == null);
            modelBuilder.Entity<User>().HasQueryFilter(x => x.DeletedAt == null);
            modelBuilder.Entity<RefreshToken>().HasQueryFilter(x => x.DeletedAt == null);
            modelBuilder.Entity<RolePermission>().HasQueryFilter(x => x.DeletedAt == null);
            modelBuilder.Entity<RoleUser>().HasQueryFilter(x => x.DeletedAt == null);
            modelBuilder.Entity<PermissionEntity>().HasQueryFilter(x => x.DeletedAt == null);
            modelBuilder.Entity<Staff>().HasQueryFilter(x => x.DeletedAt == null);
            modelBuilder.Entity<DepartmentStaff>().HasQueryFilter(x => x.DeletedAt == null);
            modelBuilder.Entity<DepartmentStaffHistory>().HasQueryFilter(x => x.DeletedAt == null);
            modelBuilder.Entity<Park>().HasQueryFilter(x => x.DeletedAt == null);
            modelBuilder.Entity<Location>().HasQueryFilter(x => x.DeletedAt == null);
            modelBuilder.Entity<Species>().HasQueryFilter(x => x.DeletedAt == null);
            modelBuilder.Entity<Vegetation>().HasQueryFilter(x => x.DeletedAt == null);
            modelBuilder.Entity<Disturbance>().HasQueryFilter(x => x.DeletedAt == null);
            modelBuilder.Entity<DistanceSample>().HasQueryFilter(x => x.DeletedAt == null);
            modelBuilder.Entity<SpeciesOccurrence>().HasQueryFilter(x => x.DeletedAt == null);
            modelBuilder.Entity<LifeForm>().HasQueryFilter(x => x.DeletedAt == null);
            modelBuilder.Entity<Invasive>().HasQueryFilter(x => x.DeletedAt == null);
            modelBuilder.Entity<EcologySelection>().HasQueryFilter(x => x.DeletedAt == null);
            modelBuilder.Entity<RoadKill>().HasQueryFilter(x => x.DeletedAt == null);
            modelBuilder.Entity<AnimalDemographic>().HasQueryFilter(x => x.DeletedAt == null);
            modelBuilder.Entity<Station>().HasQueryFilter(x => x.DeletedAt == null);
            modelBuilder.Entity<Weather>().HasQueryFilter(x => x.DeletedAt == null);
            modelBuilder.Entity<Waste>().HasQueryFilter(x => x.DeletedAt == null);
            modelBuilder.Entity<WasteMaterial>().HasQueryFilter(x => x.DeletedAt == null);
            modelBuilder.Entity<UserPark>().HasQueryFilter(x => x.DeletedAt == null);
            modelBuilder.Entity<HabitatManipulation>().HasQueryFilter(x => x.DeletedAt == null);
            modelBuilder.Entity<AerialCensus>().HasQueryFilter(x => x.DeletedAt == null);
            modelBuilder.Entity<WaterBody>().HasQueryFilter(x => x.DeletedAt == null);
            modelBuilder.Entity<LessLivestockDaily>().HasQueryFilter(x => x.DeletedAt == null);
            modelBuilder.Entity<LessLivestockDailyLivestock>().HasQueryFilter(x => x.DeletedAt == null);
            modelBuilder.Entity<LessLivestockDailyAction>().HasQueryFilter(x => x.DeletedAt == null);
            modelBuilder.Entity<LessOperationalZone>().HasQueryFilter(x => x.DeletedAt == null);
            modelBuilder.Entity<LessRangerStation>().HasQueryFilter(x => x.DeletedAt == null);
            modelBuilder.Entity<LessRangerGroup>().HasQueryFilter(x => x.DeletedAt == null);
            modelBuilder.Entity<LessRangerRankCategory>().HasQueryFilter(x => x.DeletedAt == null);
            modelBuilder.Entity<LessStaffPosting>().HasQueryFilter(x => x.DeletedAt == null);
            modelBuilder.Entity<LessRangerDailyDivision>().HasQueryFilter(x => x.DeletedAt == null);
            modelBuilder.Entity<LessRangerDailyDivisionAssignment>().HasQueryFilter(x => x.DeletedAt == null);
            modelBuilder.Entity<LessPatrolDaily>().HasQueryFilter(x => x.DeletedAt == null);
            modelBuilder.Entity<LessRangerDutyFieldDefinition>().HasQueryFilter(x => x.DeletedAt == null);
            modelBuilder.Entity<LessActivity>().HasQueryFilter(x => x.DeletedAt == null);
            modelBuilder.Entity<LessLivestockType>().HasQueryFilter(x => x.DeletedAt == null);
            modelBuilder.Entity<LessLivestockActionOption>().HasQueryFilter(x => x.DeletedAt == null);

            modelBuilder.Entity<LessHwcTabDefinition>().HasQueryFilter(x => x.DeletedAt == null);
            modelBuilder.Entity<LessHwcFieldDefinition>().HasQueryFilter(x => x.DeletedAt == null);
            modelBuilder.Entity<LessHwcFieldOption>().HasQueryFilter(x => x.DeletedAt == null);
            modelBuilder.Entity<LessHwcIncident>().HasQueryFilter(x => x.DeletedAt == null);

            modelBuilder.Entity<WaterQuality>().HasQueryFilter(x => x.DeletedAt == null);
            modelBuilder.Entity<WaterQuantity>().HasQueryFilter(x => x.DeletedAt == null);
            modelBuilder.Entity<FileEntity>().HasQueryFilter(x => x.DeletedAt == null);
            modelBuilder.Entity<FirePrescription>().HasQueryFilter(x => x.DeletedAt == null);
            modelBuilder.Entity<WildFire>().HasQueryFilter(x => x.DeletedAt == null);
            modelBuilder.Entity<FireBreak>().HasQueryFilter(x => x.DeletedAt == null);
            modelBuilder.Entity<FireSeminar>().HasQueryFilter(x => x.DeletedAt == null);
            modelBuilder.Entity<SightingTurtle>().HasQueryFilter(x => x.DeletedAt == null);
            modelBuilder.Entity<NestingTurtle>().HasQueryFilter(x => x.DeletedAt == null);
            modelBuilder.Entity<DeathTurtle>().HasQueryFilter(x => x.DeletedAt == null);
            modelBuilder.Entity<LineTransect>().HasQueryFilter(x => x.DeletedAt == null);
            modelBuilder.Entity<MigratoryBird>().HasQueryFilter(x => x.DeletedAt == null);
            modelBuilder.Entity<BirdSurvey>().HasQueryFilter(x => x.DeletedAt == null);
            modelBuilder.Entity<Region>().HasQueryFilter(x => x.DeletedAt == null);
            modelBuilder.Entity<District>().HasQueryFilter(x => x.DeletedAt == null);
            modelBuilder.Entity<Division>().HasQueryFilter(x => x.DeletedAt == null);
            modelBuilder.Entity<Ward>().HasQueryFilter(x => x.DeletedAt == null);
            modelBuilder.Entity<Village>().HasQueryFilter(x => x.DeletedAt == null);
            modelBuilder.Entity<RareEndangeredSpecies>().HasQueryFilter(x => x.DeletedAt == null);
            modelBuilder.Entity<RareSpeciesOccurrence>().HasQueryFilter(x => x.DeletedAt == null);
            modelBuilder.Entity<GroundCount>().HasQueryFilter(x => x.DeletedAt == null);
            modelBuilder.Entity<GroundCountSighting>().HasQueryFilter(x => x.DeletedAt == null);
            modelBuilder.Entity<MangabeyObservation>().HasQueryFilter(x => x.DeletedAt == null);
            modelBuilder.Entity<MangabeyEatingBehavior>().HasQueryFilter(x => x.DeletedAt == null);
            modelBuilder.Entity<MangabeyMatingBehavior>().HasQueryFilter(x => x.DeletedAt == null);
            modelBuilder.Entity<MangabeyFightingBehavior>().HasQueryFilter(x => x.DeletedAt == null);
            modelBuilder.Entity<MangabeyOtherSpecieObservation>().HasQueryFilter(x => x.DeletedAt == null);
            modelBuilder.Entity<MangabeyMonitoring>().HasQueryFilter(x => x.DeletedAt == null);
            modelBuilder.Entity<VillageProfile>().HasQueryFilter(x => x.DeletedAt == null);
            modelBuilder.Entity<GovernmentLeader>().HasQueryFilter(x => x.DeletedAt == null);
            modelBuilder.Entity<CommunitySelection>().HasQueryFilter(x => x.DeletedAt == null);
            modelBuilder.Entity<Tribe>().HasQueryFilter(x => x.DeletedAt == null);
            modelBuilder.Entity<DistrictProfile>().HasQueryFilter(x => x.DeletedAt == null);
            modelBuilder.Entity<DistrictContext>().HasQueryFilter(x => x.DeletedAt == null);
            modelBuilder.Entity<DevelopmentOrganization>().HasQueryFilter(x => x.DeletedAt == null);
        }

    }
}
