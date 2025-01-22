using Artemis.Backend.Core.DTO.Setup;

namespace Artemis.Backend.Core.DTO.Materials
{
    public class MaterialDTO
    {
        public int Id { get; set; }
        public string PartNumber { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public string? Status { get; set; }
        public decimal? StdCost { get; set; }
        public DateTime InsertDate { get; set; }
        public string UserName { get; set; } = string.Empty;
        public BusinessDTO Business { get; set; } = null!;
        public int Version { get; set; }
        public DateTime EffectiveDate { get; set; }
        public DateTime? ObsoleteDate { get; set; }
        public ICollection<MaterialPropertiesDTO>? Properties { get; set; }
        public ICollection<MaterialLocationDTO>? Locations { get; set; }
        public ICollection<MaterialHistoryDTO>? History { get; set; }
    }
}
