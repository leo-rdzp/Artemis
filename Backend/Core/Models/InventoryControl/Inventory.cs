using Artemis.Backend.Core.Models.Authentication;
using Artemis.Backend.Core.Models.Setup;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Artemis.Backend.Core.Models.InventoryControl
{
    [Table("Inventory")]
    public class Inventory
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

        [ForeignKey("UserId")]
        public required User User { get; set; }

        [Required]
        public required DateTime InsertDate { get; set; }

        [Required]
        public required DateTime UpdateDate { get; set; }

        [ForeignKey("BusinessId")]
        public required Business Business { get; set; }

        public ICollection<InventoryAreaMap>? AreaMaps { get; set; }

        public ICollection<InventoryMaterialMap>? MaterialMaps { get; set; }

        public ICollection<InventoryProductMap>? ProductMaps { get; set; }

        public ICollection<Folio>? Folios { get; set; }
    }
}
