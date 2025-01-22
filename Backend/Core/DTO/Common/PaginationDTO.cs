namespace Artemis.Backend.Core.DTO.Common
{
    public class PaginationDTO<T>
    {
        public IEnumerable<T> Items { get; set; } = [];
        public PaginationMetaDTO Meta { get; set; } = new();
    }

    public class PaginationMetaDTO
    {
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public int TotalItems { get; set; }
        public int TotalPages { get; set; }
        public bool HasPrevious { get; set; }
        public bool HasNext { get; set; }
    }
}
