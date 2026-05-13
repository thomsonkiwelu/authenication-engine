namespace authentication_engine.Features.Ranks
{
    public record RankDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public int Level { get; set; }
        public DateTime CreatedAt { get; set; }
        public Guid? CreatedBy { get; set; }
    }

    public record RankRequest(
        string Name,
        string Code,
        string Category
    );

    public record RankResponseDto(
        Guid Id,
        int RowNumber,
        string Name,
        string Code,
        string Category,
        int Level,
        DateTime CreatedAt
        //DateTime CreatedBy
    );
}
