using Artemis.Backend.Core.DTO.OrderProcessing;
using Artemis.Backend.Core.DTO.ProcessControl;
using Artemis.Backend.Core.DTO.Setup;

namespace Artemis.Backend.Core.DTO.Devices
{
    public class PreloadDTO
    {
        public int Id { get; set; }
        public OrderLineDTO? OrderLine { get; set; }
        public string Identifier { get; set; } = string.Empty;
        public ProductDTO? Product { get; set; }
        public DispositionDTO Disposition { get; set; } = null!;
        public string Used { get; set; } = string.Empty;
        public DateTime InsertDate { get; set; }
        public ICollection<PreloadPropertiesDTO>? Properties { get; set; }
    }
}
