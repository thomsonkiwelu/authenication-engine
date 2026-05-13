namespace conservation_backend.Shared
{
    public record PaginationDto
    {
        private const int MaxPageSize = 100;

        public int page { get; set; } = 1;

        private int _pageSize = 15;

        public int pageSize
        {
            get => _pageSize;
            set => _pageSize = value <= 0 ? 15 : Math.Min(value, MaxPageSize);
        }

        public string? q { get; set; } = null;

        public string? sortBy { get; set; } = null;

        public bool sortDesc { get; set; } = true;
    }
}
