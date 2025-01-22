using Artemis.Backend.Core.DTO.OrderProcessing;
using Artemis.Backend.Core.DTO.Packaging;
using Artemis.Backend.Core.DTO.ProcessControl;
using Artemis.Backend.Core.DTO.Setup;

namespace Artemis.Backend.Core.DTO.Devices
{
    public class DeviceDTO
    {
        public int Id { get; set; }
        public string Identifier { get; set; } = string.Empty;
        public ProductDTO Product { get; set; } = null!;
        public OperationDTO LastOperation { get; set; } = null!;
        public string PassFailIndicator { get; set; } = string.Empty;
        public string OperationStation { get; set; } = string.Empty;
        public DispositionDTO Disposition { get; set; } = null!;
        public DateTime UpdateDate { get; set; }
        public string? Status { get; set; }
        public DateTime LoginDate { get; set; }
        public DateTime? ShipDate { get; set; }
        public PackageContainmentDTO? Package { get; set; }
        public string? BillRepairLevel { get; set; }
        public string Shipped { get; set; } = string.Empty;
        public BusinessDTO Business { get; set; } = null!;
        public ICollection<DevicePropertiesDTO>? Properties { get; set; }
        public ICollection<DeviceHistoryDTO>? History { get; set; }
        public ICollection<DeviceConsumptionDTO>? Consumptions { get; set; }
        public ICollection<ContainmentDTO>? ParentContainments { get; set; }
        public ICollection<ContainmentDTO>? ChildContainments { get; set; }
        public ICollection<OrderLineDTO>? OrderLines { get; internal set; }
    }
}
