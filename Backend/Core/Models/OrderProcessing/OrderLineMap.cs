using Artemis.Backend.Core.Models.Devices;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Artemis.Backend.Core.Models.OrderProcessing
{
    [Table("OrderLineMap")]
    public class OrderLineMap
    {
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public required int Id { get; set; }

        [ForeignKey("OrderLineId")]
        public required OrderLine OrderLine { get; set; }

        [ForeignKey("DeviceId")]
        public required Device Device { get; set; }

        [Required]
        [StringLength(15)]
        public required string Type { get; set; }

        [Required]
        public required DateTime InsertDate { get; set; }
    }
}
