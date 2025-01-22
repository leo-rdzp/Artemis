using Artemis.Backend.Core.DTO.Authentication;

namespace Artemis.Backend.Core.DTO.Packaging
{
    public class PackageContainmentDTO
    {
        public int Id { get; set; }
        public PackageDTO Package { get; set; } = null!;
        public string ContentType { get; set; } = string.Empty;
        public int ContentId { get; set; }
        public int Quantity { get; set; }
        public string Status { get; set; } = string.Empty;
        public DateTime InsertDate { get; set; }
        public DateTime? RemovalDate { get; set; }
        public UserDTO User { get; set; } = null!;
    }
}
