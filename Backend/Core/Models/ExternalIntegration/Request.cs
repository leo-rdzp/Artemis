using Artemis.Backend.Core.Models.Setup;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Artemis.Backend.Core.Models.ExternalIntegration
{
    [Table("Request")]
    public class Request
    {
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public required int Id { get; set; }

        [Required]
        [StringLength(30)]
        public required string Number { get; set; }

        [Required]
        [StringLength(20)]
        public required string Type { get; set; }

        [Required]
        [StringLength(15)]
        public required string Status { get; set; }

        [ForeignKey("BusinessId")]
        public required Business Business { get; set; }

        [Required]
        public required DateTime InsertDate { get; set; }

        public ICollection<RequestProperties>? Properties { get; set; }
    }
}
