using authentication_engine.Data.Seeders;
using authentication_engine.Features.Auth;
using authentication_engine.Features.Auth.Interfaces;
using authentication_engine.Features.Auth.Services;
using authentication_engine.Features.Auth.Validators;
using authentication_engine.Features.Departments;
using authentication_engine.Features.Departments.Interfaces;
using authentication_engine.Features.LessOperationalZones;
using authentication_engine.Features.LessOperationalZones.Interfaces;
using authentication_engine.Features.LessRangerGroups;
using authentication_engine.Features.LessRangerGroups.Interfaces;
using authentication_engine.Features.LessRangerStations;
using authentication_engine.Features.LessRangerStations.Interfaces;
using authentication_engine.Features.LessStaffPostings;
using authentication_engine.Features.LessStaffPostings.Interfaces;
using authentication_engine.Features.Offices;
using authentication_engine.Features.Offices.Interface;
using authentication_engine.Features.Parks;
using authentication_engine.Features.Parks.Interfaces;
using authentication_engine.Features.Parks.Validators;
using authentication_engine.Features.Permissions;
using authentication_engine.Features.Permissions.Interfaces;
using authentication_engine.Features.Ranks;
using authentication_engine.Features.Ranks.Interfaces;
using authentication_engine.Features.Roles;
using authentication_engine.Features.Roles.Interfaces;
using authentication_engine.Features.Roles.Validations;
using authentication_engine.Features.Sections;
using authentication_engine.Features.Sections.Interfaces;
using authentication_engine.Features.Staffs;
using authentication_engine.Features.Staffs.Interfaces;
using authentication_engine.Features.Stations;
using authentication_engine.Features.Stations.Interfaces;
using authentication_engine.Features.Structure;
using authentication_engine.Features.Structure.Interfaces;
using authentication_engine.Features.SystemApplications;
using authentication_engine.Features.SystemApplications.Interfaces;
using authentication_engine.Features.SystemModules;
using authentication_engine.Features.SystemModules.Interfaces;
using authentication_engine.Features.Units;
using authentication_engine.Features.Units.Interfaces;
using authentication_engine.Features.Users;
using authentication_engine.Features.Users.Interfaces;
using authentication_engine.Shared;
using FluentValidation;

namespace authentication_engine.Extensions
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
            services.AddScoped<IBaseSeeder, SystemApplicationSeeder>();
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
            services.AddScoped<ISystemApplicationRepository, SystemApplicationRepository>();

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
            services.AddSingleton<IEncryptionService, EncryptionService>();
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
            services.AddScoped<ISystemApplicationService, SystemApplicationService>();

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
            services.AddScoped<IValidator<AssignParkToUserRequest>, AssignParkToUserValidator>();

            return services;
        }
    }
}
