using Artemis.Backend.Core.Models.Authentication;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Artemis.Backend.Core.Models.Materials
{
    [Table("BomRevisionHistory")]
    public class BomRevisionHistory
    {
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public required int Id { get; set; }

        [Required]
        [ForeignKey("BomHeaderId")]
        public required BomHeader BomHeader { get; set; }

        [Required]
        [StringLength(16)]
        public required string RevisionNumber { get; set; }

        [Required]
        [StringLength(20)]
        public required string ChangeType { get; set; }

        [Required]
        [StringLength(255)]
        public required string Description { get; set; }

        [Required]
        [ForeignKey("ChangedBy")]
        public required User ChangedByUser { get; set; }

        [Required]
        public required DateTime ChangeDate { get; set; }
    }
}
