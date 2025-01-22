using Artemis.Backend.Core.Models.Setup;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Artemis.Backend.Core.Models.Devices
{
    [Table("DeviceBlacklist")]
    public class DeviceBlacklist
    {
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public required int Id { get; set; }

        [Required]
        [StringLength(40)]
        public required string SerialNumber { get; set; }

        [ForeignKey("ProductId")]
        public Product? Product { get; set; }

        [Required]
        [StringLength(15)]
        public required string Status { get; set; }

        [Required]
        public required DateTime InsertDate { get; set; }

        [ForeignKey("BusinessId")]
        public required Business Business { get; set; }
    }
}
