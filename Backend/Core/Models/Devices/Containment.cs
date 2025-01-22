using Artemis.Backend.Core.Models.Authentication;
using Artemis.Backend.Core.Models.Materials;
using Artemis.Backend.Core.Models.ProcessControl;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Artemis.Backend.Core.Models.Devices
{
    [Table("Containment")]
    public class Containment
    {
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public required int Id { get; set; }

        [ForeignKey("ParentDeviceId")]
        public required Device ParentDevice { get; set; }

        [ForeignKey("ReferenceDesignatorId")]
        public required ReferenceDesignator ReferenceDesignator { get; set; }

        [ForeignKey("ChildDeviceId")]
        public required Device ChildDevice { get; set; }

        [Required]
        public required int Quantity { get; set; }

        [ForeignKey("DispositionId")]
        public required Disposition Disposition { get; set; }

        [Required]
        [StringLength(15)]
        public required string Status { get; set; }

        [Required]
        public required DateTime InsertDate { get; set; }

        [ForeignKey("UserId")]
        public required User User { get; set; }
    }
}
