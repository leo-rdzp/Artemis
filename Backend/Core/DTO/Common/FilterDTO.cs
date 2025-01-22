namespace Artemis.Backend.Core.DTO.Common
{
    public class FilterDTO
    {
        public string? SearchTerm { get; set; }
        public string? Status { get; set; }
        public int? Page { get; set; }
        public int? PageSize { get; set; }
    }
}
