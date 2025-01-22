using Artemis.Backend.Core.Models.Setup;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Artemis.Backend.Core.Models.Devices;
using Artemis.Backend.Core.Models.Packaging;
using Artemis.Backend.Core.Models.ExternalIntegration;

namespace Artemis.Backend.Core.Models.OrderProcessing
{
    [Table("OrderLine")]
    public class OrderLine
    {
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public required int Id { get; set; }

        [ForeignKey("OrderId")]
        public required Order Order { get; set; }

        [Required]
        [StringLength(30)]
        public required string Number { get; set; }

        [ForeignKey("ProductId")]
        public required Product Product { get; set; }

        [Required]
        public required int Quantity { get; set; }

        [Required]
        [StringLength(1)]
        public required string Hold { get; set; }

        [Required]
        [StringLength(15)]
        public required string Status { get; set; }

        [Required]
        public required DateTime InsertDate { get; set; }

        [Required]
        public required DateTime UpdateDate { get; set; }

        public ICollection<OrderLineProperties>? Properties { get; set; }

        public ICollection<OrderLineMap>? OrderLineMaps { get; set; }
        [NotMapped]
        public ICollection<Device>? Devices { get; set; }

        public ICollection<Preload>? PreloadDevices { get; set; }

        public ICollection<OrderLinePackageMap>? OrderLinePackageMaps { get; set; }
        [NotMapped]
        public ICollection<Package>? Packages { get; set; }

        public ICollection<OrderLineRequestMap>? OrderLineRequestMaps { get; set; }
        [NotMapped]
        public ICollection<Request>? Requests { get; set; }
    }
}
