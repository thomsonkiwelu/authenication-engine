using authentication_engine.Features.Users;

namespace authentication_engine.Features.SystemApplications
{
    public record SystemApplicationDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Url { get; set; } = string.Empty;
        //public string ApiKey { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; } = string.Empty;
        public DateTime? UpdatedAt { get; set; }
        public string UpdatedBy { get; set; } = string.Empty;
        //Relationship
        public UserMinimalDto Creator { get; set; } = new UserMinimalDto();
        public UserMinimalDto Updater { get; set; } = new UserMinimalDto();
    }
    
    public record SystemApplicationRequestDto
    {
        public string Name { get; set; } = string.Empty;
        public string Url { get; set; } = string.Empty;
        public string ApiKey { get; set; } = string.Empty;
        public Guid CreatedBy { get; set; }
        public Guid? UpdatedBy { get; set; }
    }
    
    public record SystemApplicationResponseDto: SystemApplicationDto
    {
        public int RowNumber { get; set; }
    }
    
}
