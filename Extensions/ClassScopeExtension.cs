using conservation_backend.Data.Seeders;
using conservation_backend.Features.Auth;
using conservation_backend.Features.Auth.Interfaces;
using conservation_backend.Features.Auth.Services;
using conservation_backend.Features.Auth.Validators;
using conservation_backend.Features.Departments;
using conservation_backend.Features.Departments.Interfaces;
using conservation_backend.Features.LessOperationalZones;
using conservation_backend.Features.LessOperationalZones.Interfaces;
using conservation_backend.Features.LessRangerGroups;
using conservation_backend.Features.LessRangerGroups.Interfaces;
using conservation_backend.Features.LessRangerStations;
using conservation_backend.Features.LessRangerStations.Interfaces;
using conservation_backend.Features.LessStaffPostings;
using conservation_backend.Features.LessStaffPostings.Interfaces;
using conservation_backend.Features.Offices;
using conservation_backend.Features.Offices.Interface;
using conservation_backend.Features.Parks;
using conservation_backend.Features.Parks.Interfaces;
using conservation_backend.Features.Permissions;
using conservation_backend.Features.Permissions.Interfaces;
using conservation_backend.Features.Ranks;
using conservation_backend.Features.Ranks.Interfaces;
using conservation_backend.Features.Roles;
using conservation_backend.Features.Roles.Interfaces;
using conservation_backend.Features.Roles.Validations;
using conservation_backend.Features.Sections;
using conservation_backend.Features.Sections.Interfaces;
using conservation_backend.Features.Staffs;
using conservation_backend.Features.Staffs.Interfaces;
using conservation_backend.Features.Stations;
using conservation_backend.Features.Stations.Interfaces;
using conservation_backend.Features.Structure;
using conservation_backend.Features.Structure.Interfaces;
using conservation_backend.Features.SystemModule.Interfaces;
using conservation_backend.Features.SystemModules;
using conservation_backend.Features.SystemModules.Interfaces;
using conservation_backend.Features.Units;
using conservation_backend.Features.Units.Interfaces;
using conservation_backend.Features.Users;
using conservation_backend.Features.Users.Interfaces;
using conservation_backend.Shared;
using FluentValidation;

namespace conservation_backend.Extensions
{
    public static class ClassScopeExtension
    {
        public static IServiceCollection AddClassScope(this IServiceCollection services)
        {
            // Global user context
            services.AddScoped<IUserContext, UserContext>();
            
            // Seeders
            services.AddScoped<IBaseSeeder, RankSeeder>();
            services.AddScoped<IBaseSeeder, StaffSeeder>();
            services.AddScoped<IBaseSeeder, UserSeeder>();
            services.AddScoped<IBaseSeeder, SystemModuleSeeder>();
            services.AddScoped<IBaseSeeder, PermissionSeeder>();
            services.AddScoped<IBaseSeeder, RoleSeeder>();
            services.AddScoped<IBaseSeeder, ParkSeeder>();
            services.AddScoped<IBaseSeeder, StructureSeeder>();
            services.AddScoped<IBaseSeeder, OfficeSeeder>();
            services.AddScoped<IBaseSeeder, DepartmentSeeder>();
            //services.AddScoped<IBaseSeeder, UgallaStaffSeeder>();
           // services.AddScoped<IBaseSeeder, TanapaHqLessStaffSeeder>();
            //services.AddScoped<IBaseSeeder, UgallaLessOperationalSeeder>();
            services.AddScoped<IBaseSeeder, AuthSeeder>();
            
            // Repository
            services.AddScoped<IRoleRepository, RoleRepository>();
            services.AddScoped<IPermissionRepository, PermissionRepository>();
            services.AddScoped<IRankRepository, RankRepository>();
            services.AddScoped<IStructureRepository, StructureRepository>();
            services.AddScoped<IOfficeRepository, OfficeRepository>();
            services.AddScoped<IDepartmentRepository, DepartmentRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<ISystemModuleRepository, SystemModuleRepository>();
            services.AddScoped<ISectionRepository, SectionRepository>();
            services.AddScoped<IUnitRepository, UnitRepository>();
            services.AddScoped<IStaffRepository, StaffRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IParkRepository, ParkRepository>();
            services.AddScoped<IStationRepository, StationRepository>();
            services.AddScoped<ILessOperationalZoneRepository, LessOperationalZoneRepository>();
            services.AddScoped<ILessRangerStationRepository, LessRangerStationRepository>();
            services.AddScoped<ILessRangerGroupRepository, LessRangerGroupRepository>();
            services.AddScoped<ILessStaffPostingRepository, LessStaffPostingRepository>();

            // Service
            services.AddScoped<IRoleService, RoleService>();
            services.AddScoped<IPermissionService, PermissionService>();
            services.AddScoped<IRankService, RankService>();
            services.AddScoped<IStructureService, StructureService>();
            services.AddScoped<IOfficeService, OfficeService>();
            services.AddScoped<IDepartmentService, DepartmentService>();
            services.AddScoped<IPasswordService, PasswordService>();
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<ISystemModuleService, SystemModuleService>();
            services.AddScoped<ISectionService, SectionService>();
            services.AddScoped<IUnitService, UnitService>();
            services.AddScoped<IStaffService, StaffService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IParkService, ParkService>();
            services.AddScoped<IStationService, StationService>();
            services.AddScoped<ILessOperationalZoneService, LessOperationalZoneService>();
            services.AddScoped<ILessRangerStationService, LessRangerStationService>();
            services.AddScoped<ILessRangerGroupService, LessRangerGroupService>();
            services.AddScoped<ILessStaffPostingService, LessStaffPostingService>();

            // Validator
            services.AddScoped<IValidator<RoleRequest>, RoleValidator>();
            services.AddScoped<IValidator<AssignRolePermissionRequest>, RolePermissionValidator>();
            services.AddScoped<IValidator<RankRequest>, RankValidator>();
            services.AddScoped<IValidator<StructureRequest>, StructureValidator>();
            services.AddScoped<IValidator<LoginRequest>, LoginValidator>();
            services.AddScoped<IValidator<DepartmentRequest>, DepartmentValidator>();
            services.AddScoped<IValidator<OfficeRequest>, OfficeValidator>();
            services.AddScoped<IValidator<SectionRequest>, SectionValidator>();
            services.AddScoped<IValidator<UnitRequest>, UnitValidator>();
            services.AddScoped<IValidator<StaffRequest>, StaffValidator>();
            services.AddScoped<IValidator<StationRequest>, StationValidator>();
            services.AddScoped<IValidator<LessOperationalZoneRequest>, LessOperationalZoneValidator>();
            services.AddScoped<IValidator<LessRangerStationRequest>, LessRangerStationValidator>();
            services.AddScoped<IValidator<LessRangerGroupRequest>, LessRangerGroupValidator>();

            return services;
        }
    }
}
