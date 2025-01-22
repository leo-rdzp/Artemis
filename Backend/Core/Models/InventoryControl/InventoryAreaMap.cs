using Artemis.Backend.Core.Models.Setup;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Artemis.Backend.Core.Models.InventoryControl
{
    [Table("InventoryAreaMap")]
    public class InventoryAreaMap
    {
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public required int Id { get; set; }

        [ForeignKey("InventoryId")]
        public required Inventory Inventory { get; set; }

        [ForeignKey("AreaId")]
        public required Area Area { get; set; }

        [Required]
        public required DateTime InsertDate { get; set; }
    }
}
