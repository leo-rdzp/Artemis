using Artemis.Backend.Core.DTO.Devices;

namespace Artemis.Backend.Core.DTO.InventoryControl
{
    public class FolioSerializedDTO
    {
        public int Id { get; set; }
        public FolioDTO Folio { get; set; } = null!;
        public DeviceHistoryDTO DeviceHistory { get; set; } = null!;
        public string? Comments { get; set; }
        public DateTime InsertDate { get; set; }
    }
}
