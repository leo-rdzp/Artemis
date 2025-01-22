using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Artemis.Backend.Core.Models.Materials
{
    [Table("BomComponentMaterial")]
    public class BomComponentMaterial
    {
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public required int Id { get; set; }

        [Required]
        [ForeignKey("BomComponentId")]
        public required BomComponent BomComponent { get; set; }

        [Required]
        [ForeignKey("MaterialId")]
        public required Material Material { get; set; }

        [Required]
        public required int Priority { get; set; }

        [StringLength(255)]
        public string? UseCondition { get; set; }

        [Required]
        [StringLength(15)]
        public required string Status { get; set; }

        [Required]
        public required DateTime InsertDate { get; set; }

        [Required]
        public required DateTime UpdateDate { get; set; }

        [Required]
        [StringLength(15)]
        public required string UserName { get; set; }
    }
}
