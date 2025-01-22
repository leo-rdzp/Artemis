using Artemis.Backend.Core.Models.Materials;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Artemis.Backend.Core.Models.ProcessControl
{
    [Table("OperationRepairActionMap")]
    public class OperationRepairActionMap
    {
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public required int Id { get; set; }

        [ForeignKey("RepairActionId")]
        public required RepairAction RepairAction { get; set; }

        [ForeignKey("OperationId")]
        public required Operation Operation { get; set; }

        [Required]
        public required DateTime InsertDate { get; set; }

        [Required]
        [StringLength(15)]
        public required string UserName { get; set; }
    }
}
