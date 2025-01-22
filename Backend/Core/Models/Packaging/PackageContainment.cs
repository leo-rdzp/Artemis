using Artemis.Backend.Core.Models.Authentication;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Artemis.Backend.Core.Models.Packaging
{
    [Table("PackageContainment")]
    public class PackageContainment
    {
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public required int Id { get; set; }

        [ForeignKey("PackageId")]
        public required Package Package { get; set; }

        [Required]
        [StringLength(20)]
        public required string ContentType { get; set; }

        [Required]
        public required int ContentId { get; set; }

        [Required]
        public required int Quantity { get; set; }

        [Required]
        [StringLength(15)]
        public required string Status { get; set; }

        [Required]
        public required DateTime InsertDate { get; set; }

        public DateTime? RemovalDate { get; set; }

        [ForeignKey("UserId")]
        public required User User { get; set; }
    }
}
