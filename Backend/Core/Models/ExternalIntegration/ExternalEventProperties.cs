using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Artemis.Backend.Core.Models.ExternalIntegration
{
    [Table("ExternalEventProperties")]
    public class ExternalEventProperties
    {
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public required int Id { get; set; }

        [ForeignKey("ExternalEventId")]
        public required ExternalEvent ExternalEvent { get; set; }

        [Required]
        [StringLength(50)]
        public required string Tag { get; set; }

        [Required]
        [StringLength(255)]
        public required string Value { get; set; }

        [StringLength(255)]
        public string? Comments { get; set; }

        [Required]
        public required DateTime InsertDate { get; set; }
    }
}
