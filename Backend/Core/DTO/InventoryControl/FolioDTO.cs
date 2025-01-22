using Artemis.Backend.Core.DTO.Authentication;

namespace Artemis.Backend.Core.DTO.InventoryControl
{
    public class FolioDTO
    {
        public int Id { get; set; }
        public string Number { get; set; } = string.Empty;
        public InventoryDTO Inventory { get; set; } = null!;
        public string Station { get; set; } = string.Empty;
        public UserDTO User { get; set; } = null!;
        public DateTime InsertDate { get; set; }
        public ICollection<FolioSerializedDTO>? SerializedItems { get; set; }
        public ICollection<FolioNonSerializedDTO>? NonSerializedItems { get; set; }
    }
}
