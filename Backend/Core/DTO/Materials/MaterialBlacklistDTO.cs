using Artemis.Backend.Core.DTO.Setup;

namespace Artemis.Backend.Core.DTO.Materials
{
    public class MaterialBlacklistDTO
    {
        public int Id { get; set; }
        public string SerialNumber { get; set; } = string.Empty;
        public MaterialDTO? Material { get; set; }
        public string Status { get; set; } = string.Empty;
        public DateTime InsertDate { get; set; }
        public BusinessDTO Business { get; set; } = null!;
    }
}
