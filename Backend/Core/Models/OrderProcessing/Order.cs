using Artemis.Backend.Core.Models.Setup;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Artemis.Backend.Core.Models.OrderProcessing
{
    [Table("Order")]
    public class Order
    {
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public required int Id { get; set; }

        [Required]
        [StringLength(30)]
        public required string Number { get; set; }

        [ForeignKey("OrderTypeId")]
        public required OrderType OrderType { get; set; }

        [Required]
        public required int Quantity { get; set; }

        [Required]
        public required int CurrentQty { get; set; }

        [Required]
        [StringLength(15)]
        public required string Status { get; set; }

        [ForeignKey("CustomerId")]
        public Customer? Customer { get; set; }

        [Required]
        public required DateTime InsertDate { get; set; }

        [Required]
        public required DateTime UpdateDate { get; set; }

        [ForeignKey("BusinessId")]
        public required Business Business { get; set; }

        public ICollection<OrderLine>? OrderLines { get; set; }

        public ICollection<OrderProperties>? Properties { get; set; }

        public ICollection<OrderHistory>? History { get; set; }
    }
}
