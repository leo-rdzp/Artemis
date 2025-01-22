using Artemis.Backend.Core.DTO.Authentication;
using Artemis.Backend.Core.DTO.Materials;
using Artemis.Backend.Core.DTO.ProcessControl;

namespace Artemis.Backend.Core.DTO.Packaging
{
    public class PackageHistoryDTO
    {
        public int Id { get; set; }
        public string Action { get; set; } = string.Empty;
        public string? ContentType { get; set; }
        public int? ContentId { get; set; }
        public LocationDTO? Location { get; set; }
        public DispositionDTO Disposition { get; set; } = null!;
        public UserDTO User { get; set; } = null!;
        public DateTime InsertDate { get; set; }
        public string? Notes { get; set; }
    }
}
