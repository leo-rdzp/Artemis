using Artemis.Backend.Core.Models.ProcessControl;
using Artemis.Backend.Core.Models.Setup;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Artemis.Backend.Core.Models.OrderProcessing;

namespace Artemis.Backend.Core.Models.Devices
{
    [Table("Preload")]
    public class Preload
    {
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public required int Id { get; set; }

        [ForeignKey("OrderLineId")]
        public OrderLine? OrderLine { get; set; }

        [Required]
        [StringLength(40)]
        public required string Identifier { get; set; }

        [ForeignKey("ProductId")]
        public Product? Product { get; set; }

        [ForeignKey("DispositionId")]
        public required Disposition Disposition { get; set; }

        [Required]
        [StringLength(1)]
        public required string Used { get; set; }

        [Required]
        public required DateTime InsertDate { get; set; }

        public ICollection<PreloadProperties>? Properties { get; set; }
    }
}
