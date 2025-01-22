using Artemis.Backend.Core.Models.Authentication;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Artemis.Backend.Core.Models.OrderProcessing
{
    [Table("OrderHistory")]
    public class OrderHistory
    {
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public required int Id { get; set; }

        [ForeignKey("OrderId")]
        public required Order Order { get; set; }

        [ForeignKey("OrderLineId")]
        public required OrderLine OrderLine { get; set; }

        [Required]
        [StringLength(15)]
        public required string Status { get; set; }

        [StringLength(50)]
        public string? Comments { get; set; }

        [Required]
        public required DateTime InsertDate { get; set; }

        [ForeignKey("UserId")]
        public required User User { get; set; }
    }
}
