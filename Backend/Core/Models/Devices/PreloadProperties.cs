using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Artemis.Backend.Core.Models.Devices
{
    [Table("PreloadProperties")]
    public class PreloadProperties
    {
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public required int Id { get; set; }

        [ForeignKey("PreloadId")]
        public required Preload Preload { get; set; }

        [Required]
        [StringLength(50)]
        public required string Tag { get; set; }

        [Required]
        [StringLength(255)]
        public string? Value { get; set; }

        [Required]
        public required DateTime InsertDate { get; set; }
    }
}
