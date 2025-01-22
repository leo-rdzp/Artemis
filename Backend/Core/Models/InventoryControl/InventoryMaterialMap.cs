using Artemis.Backend.Core.Models.Materials;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Artemis.Backend.Core.Models.InventoryControl
{
    [Table("InventoryMaterialMap")]
    public class InventoryMaterialMap
    {
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public required int Id { get; set; }

        [ForeignKey("InventoryId")]
        public required Inventory Inventory { get; set; }

        [ForeignKey("MaterialId")]
        public required Material Material { get; set; }

        [Required]
        public required DateTime InsertDate { get; set; }
    }
}
