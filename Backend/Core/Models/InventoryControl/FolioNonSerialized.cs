using Artemis.Backend.Core.Models.Materials;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Artemis.Backend.Core.Models.InventoryControl
{
    [Table("FolioNonSerialized")]
    public class FolioNonSerialized
    {
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public required int Id { get; set; }

        [ForeignKey("FolioId")]
        public required Folio Folio { get; set; }

        [ForeignKey("MaterialId")]
        public required Material Material { get; set; }

        [ForeignKey("LocationId")]
        public required Location Location { get; set; }

        [Required]
        public required int Quantity { get; set; }

        [StringLength(100)]
        public string? Comments { get; set; }

        [Required]
        public required DateTime InsertDate { get; set; }
    }
}
