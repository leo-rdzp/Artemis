using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Artemis.Backend.Core.Models.Materials
{
    [Table("OnHandMaterial")]
    public class OnHandMaterial
    {
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public required int Id { get; set; }

        [ForeignKey("MaterialId")]
        public required Material Material { get; set; }

        [ForeignKey("LocationId")]
        public required Location Location { get; set; }

        [Required]
        public required decimal Quantity { get; set; }

        [Required]
        [StringLength(15)]
        public required string Status { get; set; }

        [Required]
        public required DateTime InsertDate { get; set; }
    }
}
