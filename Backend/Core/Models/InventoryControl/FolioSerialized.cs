using Artemis.Backend.Core.Models.Devices;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Artemis.Backend.Core.Models.InventoryControl
{
    [Table("FolioSerialized")]
    public class FolioSerialized
    {
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public required int Id { get; set; }

        [ForeignKey("FolioId")]
        public required Folio Folio { get; set; }

        [ForeignKey("DeviceHistoryId")]
        public required DeviceHistory DeviceHistory { get; set; }

        [StringLength(100)]
        public string? Comments { get; set; }

        [Required]
        public required DateTime InsertDate { get; set; }
    }
}
