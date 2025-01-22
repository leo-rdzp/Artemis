using Artemis.Backend.Core.Models.Authentication;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Artemis.Backend.Core.Models.InventoryControl
{
    [Table("Folio")]
    public class Folio
    {
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public required int Id { get; set; }

        [Required]
        [StringLength(30)]
        public required string Number { get; set; }

        [ForeignKey("InventoryId")]
        public required Inventory Inventory { get; set; }

        [Required]
        [StringLength(25)]
        public required string Station { get; set; }

        [ForeignKey("UserId")]
        public required User User { get; set; }

        [Required]
        public required DateTime InsertDate { get; set; }

        public ICollection<FolioSerialized>? SerializedItems { get; set; }

        public ICollection<FolioNonSerialized>? NonSerializedItems { get; set; }
    }
}
