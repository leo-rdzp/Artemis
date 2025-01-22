using Artemis.Backend.Core.Models.Authentication;
using Artemis.Backend.Core.Models.Materials;
using Artemis.Backend.Core.Models.ProcessControl;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Artemis.Backend.Core.Models.Packaging
{
    [Table("PackageHistory")]
    public class PackageHistory
    {
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public required int Id { get; set; }

        [ForeignKey("PackageId")]
        public required Package Package { get; set; }

        [Required]
        [StringLength(20)]
        public required string Action { get; set; }

        [StringLength(20)]
        public string? ContentType { get; set; }

        public int? ContentId { get; set; }

        [ForeignKey("LocationId")]
        public Location? Location { get; set; }

        [ForeignKey("DispositionId")]
        public required Disposition Disposition { get; set; }

        [ForeignKey("UserId")]
        public required User User { get; set; }

        [Required]
        public required DateTime InsertDate { get; set; }

        [StringLength(255)]
        public string? Notes { get; set; }
    }
}
