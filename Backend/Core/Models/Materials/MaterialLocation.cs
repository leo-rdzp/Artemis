using Artemis.Backend.Core.Models.Setup;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Artemis.Backend.Core.Models.Materials
{
    [Table("MaterialLocation")]
    public class MaterialLocation
    {
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public required int Id { get; set; }

        [ForeignKey("MaterialId")]
        public required Material Material { get; set; }

        [ForeignKey("LocationId")]
        public required Location Location { get; set; }

        [Required]
        public required decimal Quantity { get; set; }

        [Required]
        public required DateTime LastMovementDate { get; set; }

        public DateTime? LastCountDate { get; set; }

        public decimal? MinQuantity { get; set; }

        public decimal? MaxQuantity { get; set; }

        [Required]
        [StringLength(15)]
        public required string Status { get; set; }

        [ForeignKey("BusinessId")]
        public required Business Business { get; set; }

        public ICollection<MaterialLocationProperties>? Properties { get; set; }
    }
}
