using Artemis.Backend.Core.DTO.Authentication;
using Artemis.Backend.Core.DTO.ProcessControl;
using Artemis.Backend.Core.DTO.Setup;

namespace Artemis.Backend.Core.DTO.Devices
{
    public class DeviceHistoryDTO
    {
        public int Id { get; set; }
        public ProductDTO Product { get; set; } = null!;
        public OperationDTO Operation { get; set; } = null!;
        public string OperationStation { get; set; } = string.Empty;
        public string PassFailIndicator { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public string? Notes { get; set; }
        public DispositionDTO? Disposition { get; set; }
        public int? ShopOrder { get; set; }
        public ContainmentDTO? Containment { get; set; }
        public string? ContainmentAction { get; set; }
        public UserDTO User { get; set; } = null!;
        public DateTime InsertDate { get; set; }
        public int? EventCount { get; set; }
        public BusinessDTO Business { get; set; } = null!;
        public ICollection<DeviceResultsDTO>? Results { get; set; }
    }
}
