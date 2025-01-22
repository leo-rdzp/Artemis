using Artemis.Backend.Core.Models.Setup;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Artemis.Backend.Core.Models.ProcessControl
{
    [Table("Operation")]
    public class Operation
    {
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public required int Id { get; set; }

        [ForeignKey("AreaId")]
        public required Area Area { get; set; }

        [Required]
        [StringLength(30)]
        public required string Name { get; set; }

        [Required]
        [StringLength(15)]
        public required string Status { get; set; }

        [ForeignKey("BusinessId")]
        public required Business Business { get; set; }

        [Required]
        public required DateTime InsertDate { get; set; }

        [Required]
        [StringLength(15)]
        public required string UserName { get; set; }

        public ICollection<OperationProperties>? Properties { get; set; }

        public ICollection<OperationBomMap>? BomMap { get; set; }

        public ICollection<OperationDefectMap>? DefectMaps { get; set; }

        public ICollection<OperationRepairActionMap>? RepairActionMaps { get; set; }
    }
}
