using Artemis.Backend.Core.Models.Setup;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Artemis.Backend.Core.Models.Materials
{
    [Table("Defect")]
    public class Defect
    {
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public required int Id { get; set; }

        [Required]
        [StringLength(15)]
        public required string Code { get; set; }

        [Required]
        [StringLength(100)]
        public required string Description { get; set; }

        [ForeignKey("RepairGroupId")]
        public required RepairGroup RepairGroup { get; set; }

        [StringLength(15)]
        public string? Status { get; set; }

        [ForeignKey("BusinessId")]
        public required Business Business { get; set; }

        public int? Precedence { get; set; }

        [Required]
        public required DateTime InsertDate { get; set; }

        [Required]
        [StringLength(15)]
        public required string UserName { get; set; }
    }
}
