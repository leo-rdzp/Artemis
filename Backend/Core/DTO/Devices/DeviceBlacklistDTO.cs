using Artemis.Backend.Core.DTO.Setup;

namespace Artemis.Backend.Core.DTO.Devices
{
    public class DeviceBlacklistDTO
    {
        public int Id { get; set; }
        public string SerialNumber { get; set; } = string.Empty;
        public ProductDTO? Product { get; set; }
        public string Status { get; set; } = string.Empty;
        public DateTime InsertDate { get; set; }
        public BusinessDTO Business { get; set; } = null!;
    }
}
