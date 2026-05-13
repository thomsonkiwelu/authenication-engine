using conservation_backend.Features.Offices;
using conservation_backend.Features.Permissions;

namespace conservation_backend.Features.Structure
{

    public record StructureRequest(
        string Name,
        int Level
    );

    public record StructureResponseDto(
        Guid Id,
        int RowNumber,
        string Name,
        int Level,
        DateTime CreatedAt
    //DateTime CreatedBy
    );

    public record StructureWithOfficeDto
    {
        public Guid Id { get; init; }

        public string Name { get; init; } = string.Empty;

        public int Level { get; init; }

        public DateTime CreatedAt { get; init; }

        public List<OfficeDto> Offices { get; init; } = new List<OfficeDto>();
    }

    public record StructureDto
    {
        public Guid Id { get; init; }

        public string Name { get; init; } = string.Empty;

        public int Level { get; init; }

        public DateTime CreatedAt { get; init; }
    }

}
