using conservation_backend.Features.Auth;
using conservation_backend.Features.Departments;
using conservation_backend.Features.LessOperationalZones;
using conservation_backend.Features.LessRangerGroups;
using conservation_backend.Features.LessRangerStations;
using conservation_backend.Features.LessStaffPostings;
using conservation_backend.Features.Offices;
using conservation_backend.Features.Parks;
using conservation_backend.Features.Permissions;
using conservation_backend.Features.Ranks;
using conservation_backend.Features.Roles;
using conservation_backend.Features.Sections;
using conservation_backend.Features.Staffs;
using conservation_backend.Features.Stations;
using conservation_backend.Features.Structure;
using conservation_backend.Features.SystemModules;
using conservation_backend.Features.Units;
using conservation_backend.Features.Users;
using conservation_backend.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace conservation_backend.Config
{
    public class AppDBContext(DbContextOptions<AppDBContext> options) : DbContext(options)
    {
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
        
        public DbSet<Station> Stations { get; set; }
        
        public DbSet<UserPark> UserParks { get; set; }

        public DbSet<LessOperationalZone> LessOperationalZones { get; set; }

        public DbSet<LessRangerStation> LessRangerStations { get; set; }

        public DbSet<LessRangerGroup> LessRangerGroups { get; set; }
        
        public DbSet<LessStaffPosting> LessStaffPostings { get; set; }
        
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

            //INFO: LESS Staff Posting constraints
            modelBuilder.Entity<LessStaffPosting>(entity =>
            {
                entity.HasIndex(x => x.StaffId)
                    .IsUnique()
                    .HasFilter("\"EffectiveTo\" IS NULL AND \"DeletedAt\" IS NULL");
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
            modelBuilder.Entity<Station>().HasQueryFilter(x => x.DeletedAt == null);
            modelBuilder.Entity<UserPark>().HasQueryFilter(x => x.DeletedAt == null);
            modelBuilder.Entity<LessOperationalZone>().HasQueryFilter(x => x.DeletedAt == null);
            modelBuilder.Entity<LessRangerStation>().HasQueryFilter(x => x.DeletedAt == null);
            modelBuilder.Entity<LessRangerGroup>().HasQueryFilter(x => x.DeletedAt == null);
            modelBuilder.Entity<LessStaffPosting>().HasQueryFilter(x => x.DeletedAt == null);

        }

    }
}
