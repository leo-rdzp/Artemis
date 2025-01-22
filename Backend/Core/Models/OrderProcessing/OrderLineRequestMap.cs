using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Artemis.Backend.Core.Models.ExternalIntegration;

namespace Artemis.Backend.Core.Models.OrderProcessing
{
    [Table("OrderLineRequestMap")]
    public class OrderLineRequestMap
    {
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public required int Id { get; set; }

        [ForeignKey("OrderLineId")]
        public required OrderLine OrderLine { get; set; }

        [ForeignKey("RequestId")]
        public required Request Request { get; set; }

        [Required]
        [StringLength(15)]
        public required string Type { get; set; }

        [Required]
        public required DateTime InsertDate { get; set; }
    }
}
