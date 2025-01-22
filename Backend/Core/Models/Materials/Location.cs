using Artemis.Backend.Core.Models.Setup;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Artemis.Backend.Core.Models.InventoryControl;
using Artemis.Backend.Core.Models.Packaging;

namespace Artemis.Backend.Core.Models.Materials
{
    [Table("Location")]
    public class Location
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
        [StringLength(20)]
        public required string Type { get; set; }

        [ForeignKey("AreaId")]
        public required Area Area { get; set; }

        [ForeignKey("BusinessId")]
        public required Business Business { get; set; }

        [Required]
        public required DateTime InsertDate { get; set; }

        [Required]
        [StringLength(15)]
        public required string UserName { get; set; }

        public ICollection<LocationProperties>? Properties { get; set; }

        public ICollection<MaterialLocation>? MaterialLocations { get; set; }
        public ICollection<Package>? Packages { get; set; }
        public ICollection<PackageHistory>? PackageHistories { get; set; }
        public ICollection<FolioNonSerialized>? FolioNonSerialized { get; set; }
    }
}
