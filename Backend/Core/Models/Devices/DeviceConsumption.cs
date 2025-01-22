using Artemis.Backend.Core.Models.Materials;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Artemis.Backend.Core.Models.Devices
{
    [Table("DeviceConsumption")]
    public class DeviceConsumption
    {
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public required int Id { get; set; }

        [ForeignKey("DeviceId")]
        public required Device Device { get; set; }

        [ForeignKey("MaterialId")]
        public required Material Material { get; set; }

        [StringLength(40)]
        public string? MaterialSN { get; set; }

        [Required]
        public required int Quantity { get; set; }

        [ForeignKey("DefectId")]
        public Defect? Defect { get; set; }

        [ForeignKey("RepairActionId")]
        public required RepairAction RepairAction { get; set; }

        [Required]
        [StringLength(1)]
        public required string MatDischarged { get; set; }

        [Required]
        [StringLength(15)]
        public required string Status { get; set; }

        [Required]
        [StringLength(1)]
        public required string ClaimFlag { get; set; }

        [Required]
        public required DateTime InsertDate { get; set; }

        [Required]
        public required DateTime UpdateDate { get; set; }
    }
}
