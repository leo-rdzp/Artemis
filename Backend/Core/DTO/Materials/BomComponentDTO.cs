using Artemis.Backend.Core.DTO.ProcessControl;

namespace Artemis.Backend.Core.DTO.Materials
{
    public class BomComponentDTO
    {
        public int Id { get; set; }
        public int BomHeaderId { get; set; }
        public int? ParentComponentId { get; set; }
        public int Level { get; set; }
        public string? Position { get; set; }
        public decimal Quantity { get; set; }
        public decimal? MinQuantity { get; set; }
        public decimal? MaxQuantity { get; set; }
        public int? ReferenceDesignatorId { get; set; }
        public string? ReferenceDesignatorName { get; set; }
        public bool Required { get; set; }
        public string? Notes { get; set; }
        public DateTime InsertDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public string UserName { get; set; } = string.Empty;
        public ICollection<BomComponentMaterialDTO>? Materials { get; set; }
        public ICollection<BomUsageRuleDTO>? UsageRules { get; set; }
        public ICollection<OperationDTO>? Operations { get; set; }
    }
}
