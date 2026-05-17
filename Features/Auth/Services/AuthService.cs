using authentication_engine.Features.Auth.Interfaces;
using authentication_engine.Features.Staffs.Interfaces;
using authentication_engine.Features.SystemApplications.Interfaces;
using authentication_engine.Features.Users;
using authentication_engine.Features.Users.Interfaces;
using authentication_engine.Shared;
using MapsterMapper;

namespace authentication_engine.Features.Auth.Services
{
    public class AuthService(
        IPasswordService passwordService,
        ITokenService tokenService,
        IUserRepository userRepository,
        IStaffRepository staffRepository,
        IEncryptionService encryptionService,
        ISystemApplicationRepository systemApplicationRepository,
        IConfiguration configuration
    ) : IAuthService
    {
        private readonly IUserRepository _userRepository = userRepository;
        private readonly IPasswordService _passwordService = passwordService;
        private readonly ITokenService _tokenService = tokenService;
        private readonly IStaffRepository _staffRepository = staffRepository;
        private readonly IEncryptionService _encryptionService = encryptionService;
        private readonly ISystemApplicationRepository _systemApplicationRepository = systemApplicationRepository;
        private readonly IConfiguration _configuration = configuration;

        public async Task<AuthResponse> Login(LoginRequest dto)
        {
            var user = await _userRepository.GetUserByUsername(dto.Username);
            if (user == null || !_passwordService.VerifyPassword(dto.Password, user.Password))
                throw new UnauthorizedAccessException(ResponseMessages.InvalidCredential);
            
            string? grantType = _configuration["AuthSetting:GrandType"];
            if (string.IsNullOrEmpty(grantType)) throw new KeyNotFoundException(ResponseMessages.GrantTypeNotConfigured);
            
            var systemApplication = await _systemApplicationRepository.GetSystemApplicationIfUserHasAccess(grantType, user.Id);
            if (systemApplication == null) throw new UnauthorizedAccessException(ResponseMessages.ApplicationAuthentication);
            
            var staff = await _staffRepository.GetById(user.StaffId);
            var accessToken = _tokenService.GenerateAccessToken(user, staff);
            
            return new AuthResponse
            {
                AccessToken = accessToken,
                User = new UserDto
                {
                    Id = user.Id,
                    Email = user.Email,
                    Username = user.Username,
                    IsActive = user.IsActive
                }
            };
        }
        
        public async Task<ThirdPartyVerifyResponse> ThirdPartyVerify(ThirdPartyVerifyRequest dto)
        {
            var user = await _userRepository.GetUserByUsername(dto.Username);
            if (user == null || !_passwordService.VerifyPassword(dto.Password, user.Password))
                throw new UnauthorizedAccessException(ResponseMessages.InvalidCredential);
            
            var systemApplication = await _systemApplicationRepository.GetSystemApplicationIfUserHasAccess(dto.GrantType, user.Id);
            if (systemApplication == null)
                throw new UnauthorizedAccessException(ResponseMessages.ApplicationAuthentication);
            
            var result = await _userRepository.GetUserAccessControl(user.Id, systemApplication.Id);
            var encryptMessage = _encryptionService.Encrypt(result!);
            
            return new ThirdPartyVerifyResponse { Message = encryptMessage, Result = result };
        }
    }
}
