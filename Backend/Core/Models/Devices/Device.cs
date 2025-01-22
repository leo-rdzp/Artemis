using Artemis.Backend.Core.Models.Packaging;
using Artemis.Backend.Core.Models.ProcessControl;
using Artemis.Backend.Core.Models.Setup;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Artemis.Backend.Core.Models.OrderProcessing;

namespace Artemis.Backend.Core.Models.Devices
{
    [Table("Device")]
    public class Device
    {
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public required int Id { get; set; }

        [Required]
        [StringLength(40)]
        public required string Identifier { get; set; }

        [ForeignKey("ProductId")]
        public required Product Product { get; set; }

        [ForeignKey("LastOperationId")]
        public required Operation LastOperation { get; set; }

        [Required]
        [StringLength(1)]
        public required string PassFailIndicator { get; set; }

        [Required]
        [StringLength(25)]
        public required string OperationStation { get; set; }

        [ForeignKey("DispositionId")]
        public required Disposition Disposition { get; set; }

        [Required]
        public required DateTime UpdateDate { get; set; }

        [StringLength(10)]
        public string? Status { get; set; }

        [Required]
        public required DateTime LoginDate { get; set; }

        public DateTime? ShipDate { get; set; }

        [ForeignKey("PackageId")]
        public PackageContainment? Package { get; set; }

        [StringLength(10)]
        public string? BillRepairLevel { get; set; }

        [Required]
        [StringLength(1)]
        public required string Shipped { get; set; }

        [ForeignKey("BusinessId")]
        public required Business Business { get; set; }

        public ICollection<DeviceProperties>? Properties { get; set; }

        public ICollection<DeviceConsumption>? Consumptions { get; set; }

        public ICollection<Containment>? ParentContainments { get; set; }

        public ICollection<Containment>? ChildContainments { get; set; }

        public ICollection<DeviceHistory>? History { get; set; }

        public ICollection<OrderLineMap>? OrderLineMaps { get; set; }
        
        [NotMapped]
        public ICollection<OrderLine>? OrderLines { get; set; }
    }
}
