using Artemis.Backend.Core.Models.Authentication;
using Artemis.Backend.Core.Models.ProcessControl;
using Artemis.Backend.Core.Models.Setup;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Artemis.Backend.Core.Models.InventoryControl;

namespace Artemis.Backend.Core.Models.Devices
{
    [Table("DeviceHistory")]
    public class DeviceHistory
    {
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public required int Id { get; set; }

        [ForeignKey("DeviceId")]
        public required Device Device { get; set; }

        [ForeignKey("ProductId")]
        public required Product Product { get; set; }

        [ForeignKey("OperationId")]
        public required Operation Operation { get; set; }

        [Required]
        [StringLength(25)]
        public required string OperationStation { get; set; }

        [Required]
        [StringLength(1)]
        public required string PassFailIndicator { get; set; }

        [Required]
        [StringLength(15)]
        public required string Status { get; set; }

        [StringLength(255)]
        public string? Notes { get; set; }

        [ForeignKey("DispositionId")]
        public Disposition? Disposition { get; set; }

        public int? ShopOrder { get; set; }

        [ForeignKey("ContainmentId")]
        public Containment? Containment { get; set; }

        [StringLength(20)]
        public string? ContainmentAction { get; set; }

        [ForeignKey("UserId")]
        public required User User { get; set; }

        [Required]
        public required DateTime InsertDate { get; set; }

        public int? EventCount { get; set; }

        [ForeignKey("BusinessId")]
        public required Business Business { get; set; }

        public ICollection<DeviceResults>? Results { get; set; }
        public ICollection<FolioSerialized>? FolioSerialized { get; set; }
    }
}
