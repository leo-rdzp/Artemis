namespace Artemis.Backend.Core.DTO.Materials
{
    public class BomHeaderDTO
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public int BusinessId { get; set; }
        public string Revision { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public string? Description { get; set; }
        public DateTime EffectiveDate { get; set; }
        public DateTime? ObsoleteDate { get; set; }
        public DateTime InsertDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public string UserName { get; set; } = string.Empty;
        public ICollection<BomComponentDTO>? Components { get; set; }
    }
}
