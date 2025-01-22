using Artemis.Backend.Core.Models.Devices;
using Artemis.Backend.Core.Models.Materials;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Artemis.Backend.Core.Models.ProcessControl;
using Artemis.Backend.Core.Models.InventoryControl;

namespace Artemis.Backend.Core.Models.Setup
{
    [Table("Product")]
    public class Product
    {
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public required int Id { get; set; }
        [Required]
        [StringLength(30)]
        public required string Name { get; set; }
        [Required]
        [StringLength(100)]
        public required string Description { get; set; }
        [Required]
        [StringLength(15)]
        public required string Status { get; set; }
        [Required]
        public required DateTime InsertDate { get; set; }
        [Required]
        [StringLength(15)]
        public required string UserName { get; set; }
        [Required]
        public required int BusinessId { get; set; }
        [ForeignKey("BusinessId")]
        public required Business Business { get; set; }
        public ICollection<ProductProperties>? Properties { get; set; }
        public ICollection<Device>? Devices { get; set; }
        public ICollection<DeviceBlacklist>? DeviceBlacklist { get; set; }
        public ICollection<Preload>? Preloads { get; set; }
        public ICollection<InventoryProductMap>? InventoryMaps { get; set; }
        public ICollection<BomHeader>? Bom { get; set; }
        public ICollection<Routing>? Routing { get; set; }
    }
}
