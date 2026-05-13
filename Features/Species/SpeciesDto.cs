
using conservation_backend.Features.Users;
using conservation_backend.Shared;

namespace conservation_backend.Features.Species
{
    public record SpeciesPaginationDto : PaginationDto
    {
        public int? Type { get; init; }
    }
    
    public record SpeciesRequest(
        string CommonName,
        string ScientificName,
        int Type
    );
    
    public record SpeciesDto
    {
        public Guid Id { get; init; }

        public string CommonName { get; init; } = string.Empty;
        
        public string ScientificName { get; init; } = string.Empty;

        public int Type { get; init; }

        public DateTime CreatedAt { get; init; }
        
        public string CreatedBy { get; init; }
    }
    
    public record SpeciesResponseDto : SpeciesDto
    {
        public int RowNumber { get; init; }
        
        public UserMinimalDto Creator { get; set; } = new UserMinimalDto();
    }
    
}
