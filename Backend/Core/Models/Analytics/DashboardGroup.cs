using Artemis.Backend.Core.Models.Authentication;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Artemis.Backend.Core.Models.Analytics
{
    [Table("DashboardGroup")]
    public class DashboardGroup
    {
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public required int Id { get; set; }

        [Required]
        [StringLength(30)]
        public required string Name { get; set; }

        [Required]
        [StringLength(40)]
        public required string Description { get; set; }

        [ForeignKey("OwnerId")]
        public required User Owner { get; set; }

        [Required]
        [StringLength(15)]
        public required string Status { get; set; }

        [Required]
        public required DateTime InsertDate { get; set; }

        public ICollection<AccessDashboardMap>? AccessMaps { get; set; }

        public ICollection<DashboardGroupMap>? GroupMaps { get; set; }
    }
}
