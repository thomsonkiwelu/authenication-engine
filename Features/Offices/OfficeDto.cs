using conservation_backend.Features.Structure;
using conservation_backend.Shared;

namespace conservation_backend.Features.Offices
{
    public record OfficePaginationDto : PaginationDto
    {
        public string? StructureId { get; init; }
    }

    public record OfficeRequest(
        string Name,
        string Code,
        int ParentOffice,
        int HeadOfOffice,
        Guid StructureId
    );

    public record OfficeResponseDto(
        Guid Id,
        int RowNumber,
        string Name,
        string Code,
        int ParentOffice,
        int HeadOfOffice,
        Guid StructureId,
        DateTime CreatedAt
        //DateTime CreatedBy
    );

    public record OfficeDto
    {
        public Guid Id { get; init; }
        public string Name { get; init; } = string.Empty;
        public string Code { get; init; } = string.Empty;
        public int ParentOffice { get; init; }
        public int HeadOfOffice { get; init; }
        public Guid? ParkId { get; init; } = null;
        public Guid StructureId { get; init; }
        public DateTime CreatedAt { get; init; }
    }

    public record OfficeWithStructureDto
    {
        public Guid Id { get; init; }
        public string Name { get; init; } = string.Empty;
        public string Code { get; init; } = string.Empty;
        public int ParentOffice { get; init; }
        public int HeadOfOffice { get; init; }
        public Guid ParkId { get; init; }
        public Guid StructureId { get; init; }
        public DateTime CreatedAt { get; init; }
        public StructureDto? Structure { get; init; }
    }
    
    public record OfficeSeedDto
    {
        public string Name { get; init; } = string.Empty;
        public string Code { get; init; } = string.Empty;
        public int ParentOffice { get; init; }
        public string StructureName { get; init; } = string.Empty;
        public Guid? ParkId { get; init; } = null;
    }
}
