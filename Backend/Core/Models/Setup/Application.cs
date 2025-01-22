using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Artemis.Backend.Core.Models.Setup
{
    [Table("Application")]
    public class Application
    {
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public required int Id { get; set; }

        [Required]
        [ForeignKey("BusinessId")]
        public required Business Business { get; set; }

        [Required]
        [StringLength(30)]
        public required string ApplicationType { get; set; }

        [Required]
        [StringLength(30)]
        public required string Station { get; set; }

        [Required]
        [StringLength(50)]
        public required string Title { get; set; }

        [Required]
        [StringLength(255)]
        public required string Route { get; set; }

        [Required]
        [StringLength(15)]
        public required string Status { get; set; }

        [Required]
        public required DateTime InsertDate { get; set; }

        [Required]
        public required DateTime UpdateDate { get; set; }

        public ICollection<ApplicationProperties>? Properties { get; set; }

        public ICollection<ApplicationRole>? ApplicationRoles { get; set; }
    }
}
