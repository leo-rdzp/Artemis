using Artemis.Backend.Core.DTO.Setup;

namespace Artemis.Backend.Core.DTO.Materials
{
    public class MaterialLocationDTO
    {
        public int Id { get; set; }
        public decimal Quantity { get; set; }
        public DateTime LastMovementDate { get; set; }
        public DateTime? LastCountDate { get; set; }
        public decimal? MinQuantity { get; set; }
        public decimal? MaxQuantity { get; set; }
        public string Status { get; set; } = string.Empty;
        public BusinessDTO Business { get; set; } = null!;
        public ICollection<MaterialLocationPropertiesDTO>? Properties { get; set; }
    }
}
