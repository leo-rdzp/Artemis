using Artemis.Backend.Core.Models.Authentication;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Artemis.Backend.Core.Models.Materials
{
    [Table("MaterialHistory")]
    public class MaterialHistory
    {
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public required int Id { get; set; }

        [ForeignKey("MaterialId")]
        public required Material Material { get; set; }

        [ForeignKey("FromLocationId")]
        public Location? FromLocation { get; set; }

        [ForeignKey("ToLocationId")]
        public required Location ToLocation { get; set; }

        [Required]
        public required decimal Quantity { get; set; }

        [Required]
        [StringLength(20)]
        public required string TransactionType { get; set; }

        [Required]
        [StringLength(20)]
        public required string ReferenceType { get; set; }

        [Required]
        public required int ReferenceId { get; set; }

        [ForeignKey("UserId")]
        public required User User { get; set; }

        [Required]
        public required DateTime InsertDate { get; set; }
    }
}
