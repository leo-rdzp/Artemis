using Artemis.Backend.Core.Models.Authentication;
using Artemis.Backend.Core.Models.InventoryControl;
using Artemis.Backend.Core.Models.Materials;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Artemis.Backend.Core.Models.ProcessControl;

namespace Artemis.Backend.Core.Models.Setup
{
    [Table("Area")]
    public class Area
    {
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public required int Id { get; set; }
        [Required]
        [StringLength(30)]
        public required string Name { get; set; }
        [StringLength(100)]
        public string? Description { get; set; }
        [Required]
        [StringLength(15)]
        public required string Status { get; set; }
        [Required]
        public required int BusinessId { get; set; }
        [Required]
        public required DateTime InsertDate { get; set; }
        [Required]
        [StringLength(15)]
        public required string UserName { get; set; }
        [ForeignKey("BusinessId")]
        public required Business Business { get; set; }
        public ICollection<Location>? Locations { get; set; }
        public ICollection<UserAreaMap>? UserAreaMaps { get; set; }
        public ICollection<InventoryAreaMap>? InventoryAreaMaps { get; set; }
        public ICollection<Operation>? Operations { get; set; }
    }
}
