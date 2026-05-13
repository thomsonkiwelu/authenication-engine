using conservation_backend.Features.Auth.Interfaces;
using conservation_backend.Features.Staffs.Interfaces;
using conservation_backend.Features.Users;
using conservation_backend.Features.Users.Interfaces;
using MapsterMapper;


namespace conservation_backend.Features.Auth.Services
{
    public class AuthService(
        IPasswordService passwordService,
        ITokenService tokenService,
        IUserRepository userRepository,
        IStaffRepository staffRepository,
        IMapper mapper
    ) : IAuthService
    {
        private readonly IUserRepository _userRepository = userRepository;
        private readonly IPasswordService _passwordService = passwordService;
        private readonly ITokenService _tokenService = tokenService;
        private readonly IMapper _mapper = mapper;
        private readonly IStaffRepository _staffRepository = staffRepository;

        public async Task<AuthResponse> Login(LoginRequest dto)
        {
            var user = await _userRepository.GetUserByUsername(dto);
            if (user == null || !_passwordService.VerifyPassword(dto.Password, user.Password))
                throw new UnauthorizedAccessException("Invalid username or password");
            
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

        public async Task<UserWithAccessControlDto> GetCurrentUser(Guid userId)
        {
            var result = await _userRepository.GetUserByIdWithAccessControl(userId);

            if (result == null)
                throw new KeyNotFoundException($"User with ID {userId} not found");
         
            return result;
        }
    }
}
