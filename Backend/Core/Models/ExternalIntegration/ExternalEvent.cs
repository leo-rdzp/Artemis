using Artemis.Backend.Core.Models.Setup;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Artemis.Backend.Core.Models.ExternalIntegration
{
    [Table("ExternalEvent")]
    public class ExternalEvent
    {
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public required int Id { get; set; }

        [Required]
        [StringLength(5)]
        public required string Direction { get; set; }

        [Required]
        [StringLength(20)]
        public required string InterfaceType { get; set; }

        [Required]
        [StringLength(30)]
        public required string Event { get; set; }

        [Required]
        [StringLength(30)]
        public required string Process { get; set; }

        [Required]
        [StringLength(50)]
        public required string ServiceName { get; set; }

        [Required]
        [StringLength(10)]
        public required string CommunicationType { get; set; }

        [Required]
        public required DateTime EventDateRequest { get; set; }

        public DateTime? EventDateResponse { get; set; }

        [Required]
        [StringLength(20)]
        public required string EventStatus { get; set; }

        [Required]
        public required int TryCounter { get; set; }

        [Required]
        public required DateTime UpdateDate { get; set; }

        [StringLength(20)]
        public string? EventResponseCode { get; set; }

        [ForeignKey("BusinessId")]
        public required Business Business { get; set; }

        public ICollection<ExternalEventProperties>? Properties { get; set; }
    }
}
