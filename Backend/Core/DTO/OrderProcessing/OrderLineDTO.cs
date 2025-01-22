using Artemis.Backend.Core.DTO.Devices;
using Artemis.Backend.Core.DTO.ExternalIntegration;
using Artemis.Backend.Core.DTO.Packaging;
using Artemis.Backend.Core.DTO.Setup;

namespace Artemis.Backend.Core.DTO.OrderProcessing
{
    public class OrderLineDTO
    {
        public int Id { get; set; }
        public OrderDTO Order { get; set; } = null!;
        public string Number { get; set; } = string.Empty;
        public ProductDTO Product { get; set; } = null!;
        public int Quantity { get; set; }
        public string Hold { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public DateTime InsertDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public ICollection<OrderLinePropertiesDTO>? Properties { get; set; }
        public ICollection<DeviceDTO>? Devices { get; set; }
        public ICollection<PackageDTO>? Packages { get; set; }
        public ICollection<RequestDTO>? Requests { get; internal set; }
    }
}
