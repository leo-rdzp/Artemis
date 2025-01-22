using Artemis.Backend.Core.Models.Setup;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Artemis.Backend.Core.Models.ProcessControl;

namespace Artemis.Backend.Core.Models.Materials
{
    [Table("BomHeader")]
    public class BomHeader
    {
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public required int Id { get; set; }

        [Required]
        [ForeignKey("ProductId")]
        public required Product Product { get; set; }

        [Required]
        [ForeignKey("BusinessId")]
        public required Business Business { get; set; }

        [Required]
        [StringLength(16)]
        public required string Revision { get; set; }

        [Required]
        [StringLength(15)]
        public required string Status { get; set; }

        [Required]
        [StringLength(20)]
        public required string Type { get; set; }

        [StringLength(100)]
        public string? Description { get; set; }

        [Required]
        public required DateTime EffectiveDate { get; set; }

        public DateTime? ObsoleteDate { get; set; }

        [Required]
        public required DateTime InsertDate { get; set; }

        [Required]
        public required DateTime UpdateDate { get; set; }

        [Required]
        [StringLength(15)]
        public required string UserName { get; set; }

        // Navigation properties
        public ICollection<BomComponent>? Components { get; set; }
        public ICollection<BomRevisionHistory>? RevisionHistory { get; set; }
        public ICollection<OperationBomMap>? Operations { get; set; }
    }
}
