using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Artemis.Backend.Core.Models.OrderProcessing
{
    [Table("OrderLineProperties")]
    public class OrderLineProperties
    {
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public required int Id { get; set; }

        [ForeignKey("OrderLineId")]
        public required OrderLine OrderLine { get; set; }

        [Required]
        [StringLength(50)]
        public required string Tag { get; set; }

        [Required]
        [StringLength(255)]
        public required string Value { get; set; }

        [StringLength(255)]
        public string? Comments { get; set; }

        [Required]
        public required DateTime InsertDate { get; set; }
    }
}
