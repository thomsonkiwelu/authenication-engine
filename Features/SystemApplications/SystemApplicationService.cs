using authentication_engine.Features.SystemApplications.Interfaces;
using authentication_engine.Shared;
using MapsterMapper;

namespace authentication_engine.Features.SystemApplications
{
    public class SystemApplicationService(ISystemApplicationRepository repository, IMapper mapper): ISystemApplicationService
    {
        private readonly ISystemApplicationRepository _systemApplicationRepository = repository;

        private readonly IMapper _mapper = mapper;

        public async Task<PagedList<SystemApplicationResponseDto>> GetAllSystemApplications(PaginationDto dto)
        {
            var pagedData = await _systemApplicationRepository.GetPagedData(dto);

            var startIndex = (pagedData.Page - 1) * pagedData.PageSize + 1;

            var dtoList = _mapper.Map<List<SystemApplicationResponseDto>>(pagedData.Data)
                .Select((dto, index) => dto with { RowNumber = startIndex + index }).ToList();

            return new PagedList<SystemApplicationResponseDto>(
                items: dtoList,
                page: pagedData.Page,
                pageSize: pagedData.PageSize,
                totalCount: pagedData.TotalCount
            );
        }

        public async Task<SystemApplicationDto> CreateSystemApplication(SystemApplicationRequestDto dto)
        {
            var systemApplication = _mapper.Map<SystemApplication>(dto);

            var created = await _systemApplicationRepository.Create(systemApplication);

            var responseDto = _mapper.Map<SystemApplicationDto>(created);

            return responseDto;
        }

        public async Task<SystemApplicationDto> GetSystemApplicationById(Guid id)
        {
            var systemApplication = await _systemApplicationRepository.GetById(id);

            var result = _mapper.Map<SystemApplicationDto>(systemApplication);

            return result;
        }

        public async Task<SystemApplicationDto> UpdateSystemApplication(Guid id, SystemApplicationRequestDto dto)
        {
            var systemApplication = _mapper.Map<SystemApplication>(dto);

            var updated = await _systemApplicationRepository.Update(id, systemApplication);

            var responseDto = _mapper.Map<SystemApplicationDto>(updated);

            return responseDto;
        }

        public async Task<bool> DeleteSystemApplication(Guid id)
        {
            return await _systemApplicationRepository.Delete(id);
        }
        
        public async Task<bool> AssignSystemApplicationToUser(AssignSystemApplicationToUserRequest dto)
        {
            var userSystemApplication = _mapper.Map<UserSystemApplication>(dto);
            
            return await _systemApplicationRepository.AssignSystemApplicationToUser(userSystemApplication);
        }
        
        public async Task<bool> UnassignSystemApplicationToUser(AssignSystemApplicationToUserRequest dto)
        {
            var userSystemApplication = _mapper.Map<UserSystemApplication>(dto);
            
            return await _systemApplicationRepository.UnassignSystemApplicationToUser(userSystemApplication);
        }
    }
}
