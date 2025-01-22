using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Artemis.Backend.Core.Models.Materials
{
    [Table("BomUsageRule")]
    public class BomUsageRule
    {
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public required int Id { get; set; }

        [Required]
        [ForeignKey("BomComponentId")]
        public required BomComponent BomComponent { get; set; }

        [Required]
        [StringLength(30)]
        public required string RuleType { get; set; }

        [Required]
        [StringLength(255)]
        public required string Condition { get; set; }

        [Required]
        [StringLength(255)]
        public required string Value { get; set; }

        [Required]
        public required DateTime InsertDate { get; set; }

        [Required]
        public required DateTime UpdateDate { get; set; }

        [Required]
        [StringLength(15)]
        public required string UserName { get; set; }
    }
}
