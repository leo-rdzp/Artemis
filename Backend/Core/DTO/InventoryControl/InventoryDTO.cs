using Artemis.Backend.Core.DTO.Authentication;
using Artemis.Backend.Core.DTO.Materials;
using Artemis.Backend.Core.DTO.Setup;

namespace Artemis.Backend.Core.DTO.InventoryControl
{
    public class InventoryDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public UserDTO User { get; set; } = null!;
        public DateTime InsertDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public BusinessDTO Business { get; set; } = null!;
        public ICollection<AreaDTO>? Areas { get; set; }
        public ICollection<MaterialDTO>? Materials { get; set; }
        public ICollection<ProductDTO>? Products { get; set; }
        public ICollection<FolioDTO>? Folios { get; set; }
    }
}
