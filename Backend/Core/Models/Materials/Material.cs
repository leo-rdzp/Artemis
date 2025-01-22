using Artemis.Backend.Core.Models.Setup;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Artemis.Backend.Core.Models.InventoryControl;

namespace Artemis.Backend.Core.Models.Materials
{
    [Table("Material")]
    public class Material
    {
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public required int Id { get; set; }

        [Required]
        [StringLength(30)]
        public required string PartNumber { get; set; }

        [Required]
        [StringLength(100)]
        public required string Description { get; set; }

        [Required]
        [StringLength(10)]
        public required string Type { get; set; }

        [StringLength(15)]
        public string? Status { get; set; }

        public decimal? StdCost { get; set; }

        [Required]
        public required DateTime InsertDate { get; set; }

        [Required]
        [StringLength(15)]
        public required string UserName { get; set; }

        [ForeignKey("BusinessId")]
        public required Business Business { get; set; }

        [Required]
        public required int Version { get; set; }

        [Required]
        public required DateTime EffectiveDate { get; set; }

        public DateTime? ObsoleteDate { get; set; }

        public ICollection<MaterialProperties>? Properties { get; set; }

        public ICollection<MaterialLocation>? Locations { get; set; }

        public ICollection<MaterialHistory>? History { get; set; }

        public ICollection<BomHeader>? BomHeaders { get; set; }

        public ICollection<MaterialBlacklist>? Blacklist { get; set; }

        public ICollection<FolioNonSerialized>? FolioNonSerialized { get; set; }
    }
}
