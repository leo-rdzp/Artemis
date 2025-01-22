using Artemis.Backend.Core.Models.Setup;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Artemis.Backend.Core.Models.Materials
{
    [Table("RepairGroup")]
    public class RepairGroup
    {
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public required int Id { get; set; }

        [Required]
        [StringLength(50)]
        public required string Name { get; set; }

        [Required]
        [StringLength(50)]
        public required string Description { get; set; }

        [Required]
        public required DateTime InsertDate { get; set; }

        [Required]
        [StringLength(15)]
        public required string UserName { get; set; }

        [ForeignKey("BusinessId")]
        public required Business Business { get; set; }

        public ICollection<Defect>? Defects { get; set; }

        public ICollection<RepairAction>? RepairActions { get; set; }
    }
}
