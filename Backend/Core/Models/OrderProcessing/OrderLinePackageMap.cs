using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Artemis.Backend.Core.Models.Packaging;

namespace Artemis.Backend.Core.Models.OrderProcessing
{
    [Table("OrderLinePackageMap")]
    public class OrderLinePackageMap
    {
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public required int Id { get; set; }

        [ForeignKey("OrderLineId")]
        public required OrderLine OrderLine { get; set; }

        [ForeignKey("PackageId")]
        public required Package Package { get; set; }

        [Required]
        [StringLength(15)]
        public required string Type { get; set; }

        [Required]
        public required DateTime InsertDate { get; set; }
    }
}
