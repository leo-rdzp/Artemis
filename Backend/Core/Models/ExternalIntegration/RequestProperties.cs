using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Artemis.Backend.Core.Models.ExternalIntegration
{
    [Table("RequestProperties")]
    public class RequestProperties
    {
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public required int Id { get; set; }

        [ForeignKey("RequestId")]
        public required Request Request { get; set; }

        [Required]
        [StringLength(50)]
        public required string Tag { get; set; }

        [Required]
        [StringLength(160)]
        public required string Value { get; set; }

        [Required]
        public required DateTime InsertDate { get; set; }
    }
}
