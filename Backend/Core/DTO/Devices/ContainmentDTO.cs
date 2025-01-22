using Artemis.Backend.Core.DTO.Authentication;
using Artemis.Backend.Core.DTO.Materials;
using Artemis.Backend.Core.DTO.ProcessControl;

namespace Artemis.Backend.Core.DTO.Devices
{
    public class ContainmentDTO
    {
        public int Id { get; set; }
        public DeviceDTO ParentDevice { get; set; } = null!;
        public ReferenceDesignatorDTO ReferenceDesignator { get; set; } = null!;
        public DeviceDTO ChildDevice { get; set; } = null!;
        public int Quantity { get; set; }
        public DispositionDTO Disposition { get; set; } = null!;
        public string Status { get; set; } = string.Empty;
        public DateTime InsertDate { get; set; }
        public UserDTO User { get; set; } = null!;
    }
}
