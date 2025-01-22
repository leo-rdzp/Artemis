using Artemis.Backend.Core.DTO.Materials;

namespace Artemis.Backend.Core.DTO.InventoryControl
{
    public class FolioNonSerializedDTO
    {
        public int Id { get; set; }
        public FolioDTO Folio { get; set; } = null!;
        public MaterialDTO Material { get; set; } = null!;
        public LocationDTO Location { get; set; } = null!;
        public int Quantity { get; set; }
        public string? Comments { get; set; }
        public DateTime InsertDate { get; set; }
    }
}
