using Artemis.Backend.Core.Models.Authentication;
using Artemis.Backend.Core.Models.Materials;
using Artemis.Backend.Core.Models.ProcessControl;
using Artemis.Backend.Core.Models.Setup;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Artemis.Backend.Core.Models.Devices;
using Artemis.Backend.Core.Models.OrderProcessing;

namespace Artemis.Backend.Core.Models.Packaging
{
    [Table("Package")]
    public class Package
    {
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public required int Id { get; set; }

        [Required]
        [StringLength(30)]
        public required string Code { get; set; }

        [Required]
        [StringLength(20)]
        public required string Type { get; set; }

        [Required]
        [StringLength(15)]
        public required string Status { get; set; }

        [ForeignKey("LocationId")]
        public Location? Location { get; set; }

        [ForeignKey("DispositionId")]
        public required Disposition Disposition { get; set; }

        [ForeignKey("BusinessId")]
        public required Business Business { get; set; }

        [Required]
        public required DateTime InsertDate { get; set; }

        public DateTime? ClosedDate { get; set; }

        [ForeignKey("UserId")]
        public required User User { get; set; }

        public ICollection<Device>? Devices { get; set; }
        public ICollection<PackageContainment>? Containments { get; set; }
        public ICollection<PackageProperties>? Properties { get; set; }
        public ICollection<PackageHistory>? History { get; set; }

        public ICollection<OrderLinePackageMap>? OrderLinePackageMaps { get; set; }
        [NotMapped]
        public ICollection<OrderLine>? OrderLines { get; set; }
    }
}
