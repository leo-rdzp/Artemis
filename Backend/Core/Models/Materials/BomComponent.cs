using Artemis.Backend.Core.Models.ProcessControl;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Artemis.Backend.Core.Models.Materials
{
    [Table("BomComponent")]
    public class BomComponent
    {
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public required int Id { get; set; }

        [Required]
        [ForeignKey("BomHeaderId")]
        public required BomHeader BomHeader { get; set; }

        [ForeignKey("ParentComponentId")]
        public BomComponent? ParentComponent { get; set; }

        [Required]
        public required int Level { get; set; }

        [StringLength(50)]
        public string? Position { get; set; }

        [Required]
        [Column(TypeName = "decimal(10,2)")]
        public required decimal Quantity { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        public decimal? MinQuantity { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        public decimal? MaxQuantity { get; set; }

        [ForeignKey("ReferenceDesignatorId")]
        public ReferenceDesignator? ReferenceDesignator { get; set; }

        [Required]
        public required bool Required { get; set; }

        [StringLength(255)]
        public string? Notes { get; set; }

        [Required]
        public required DateTime InsertDate { get; set; }

        [Required]
        public required DateTime UpdateDate { get; set; }

        [Required]
        [StringLength(15)]
        public required string UserName { get; set; }

        // Navigation properties
        public ICollection<BomComponent>? ChildComponents { get; set; }
        public ICollection<BomComponentMaterial>? Materials { get; set; }
        public ICollection<BomUsageRule>? UsageRules { get; set; }
        public ICollection<OperationBomMap>? Operations { get; set; }
    }
}
