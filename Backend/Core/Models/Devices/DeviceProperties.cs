using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Artemis.Backend.Core.Models.Devices
{
    [Table("DeviceProperties")]
    public class DeviceProperties
    {
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public required int Id { get; set; }

        [ForeignKey("DeviceId")]
        public required Device Device { get; set; }

        [Required]
        [StringLength(50)]
        public required string Tag { get; set; }

        [StringLength(50)]
        public string? Value { get; set; }

        [Required]
        public required DateTime InsertDate { get; set; }
    }
}
