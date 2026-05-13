using conservation_backend.Config;
using conservation_backend.Features.Staffs.Interfaces;
using conservation_backend.Shared;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;

namespace conservation_backend.Features.Staffs;

public class StaffService(IStaffRepository repository, IMapper mapper) : IStaffService
{
    private readonly IStaffRepository _staffRepository = repository;
    private readonly IMapper _mapper = mapper;

    public async Task<PagedList<StaffResponseDto>> GetAllStaffsData(StaffPaginationDto dto)
    {
        var pagedData = await _staffRepository.GetPagedData(dto);

        var startIndex = (pagedData.Page - 1) * pagedData.PageSize + 1;

        var dtoList = _mapper.Map<List<StaffResponseDto>>(pagedData.Data)
            .Select((dto, index) => dto with { RowNumber = startIndex + index }).ToList();

        return new PagedList<StaffResponseDto>(
            items: dtoList,
            page: pagedData.Page,
            pageSize: pagedData.PageSize,
            totalCount: pagedData.TotalCount
        );
    }

    public async Task<StaffDto> CreateStaff(StaffRequest dto)
    {
        var staff = _mapper.Map<Staff>(dto);

        var createdStaff = await _staffRepository.Create(staff);

        var responseDto = _mapper.Map<StaffDto>(createdStaff);
        
        return responseDto;
    }

    public async Task<StaffDto> GetStaffById(Guid id)
    {
        var staff = await _staffRepository.GetById(id);

        var result = _mapper.Map<StaffDto>(staff);

        return result;
    }

    public async Task<StaffDto> UpdateStaff(Guid id, StaffRequest dto)
    {
        var staff = _mapper.Map<Staff>(dto);

        var updatedStaff = await _staffRepository.Update(id, staff);

        var responseDto = _mapper.Map<StaffDto>(updatedStaff);

        return responseDto;
    }

    public async Task<bool> DeleteStaff(Guid id)
    {
        return await _staffRepository.Delete(id);
    }
    
    public async Task<OrganizationContextDto> GetStaffsWithOrganizationContext(Guid officeId)
    {
        return await _staffRepository.GetStaffsWithOrganizationContext(officeId);
    }
}