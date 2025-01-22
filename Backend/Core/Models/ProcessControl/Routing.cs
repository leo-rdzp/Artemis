using Artemis.Backend.Core.Models.Setup;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Artemis.Backend.Core.Models.ProcessControl
{
    [Table("Routing")]
    public class Routing
    {
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public required int Id { get; set; }

        [ForeignKey("ProductId")]
        public required Product Product { get; set; }

        [ForeignKey("PreviousOperationId")]
        public required Operation PreviousOperation { get; set; }

        [ForeignKey("OperationId")]
        public required Operation Operation { get; set; }

        [StringLength(1)]
        public string? PassFailIndicator { get; set; }

        [ForeignKey("DispositionId")]
        public Disposition? Disposition { get; set; }

        [StringLength(10)]
        public string? Priority { get; set; }

        [Required]
        public required DateTime InsertDate { get; set; }

        [Required]
        [StringLength(15)]
        public required string UserName { get; set; }

        [ForeignKey("BusinessId")]
        public required Business Business { get; set; }
    }
}
