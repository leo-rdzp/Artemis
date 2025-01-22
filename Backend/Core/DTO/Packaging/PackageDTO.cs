using Artemis.Backend.Core.DTO.Authentication;
using Artemis.Backend.Core.DTO.Materials;
using Artemis.Backend.Core.DTO.OrderProcessing;
using Artemis.Backend.Core.DTO.ProcessControl;
using Artemis.Backend.Core.DTO.Setup;

namespace Artemis.Backend.Core.DTO.Packaging
{
    public class PackageDTO
    {
        public int Id { get; set; }
        public string Code { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public LocationDTO? Location { get; set; }
        public DispositionDTO Disposition { get; set; } = null!;
        public BusinessDTO Business { get; set; } = null!;
        public DateTime InsertDate { get; set; }
        public DateTime? ClosedDate { get; set; }
        public UserDTO User { get; set; } = null!;
        public ICollection<PackagePropertiesDTO>? Properties { get; set; }
        public ICollection<PackageHistoryDTO>? History { get; set; }
        public ICollection<OrderLineDTO>? OrderLines { get; internal set; }
    }
}
