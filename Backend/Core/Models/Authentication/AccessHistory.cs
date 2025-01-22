using Artemis.Backend.Core.Models.Setup;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Artemis.Backend.Core.Models.Authentication
{
    [Table("AccessHistory")]
    public class AccessHistory
    {
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public required int Id { get; set; }

        [ForeignKey("UserId")]
        public required User User { get; set; }

        [ForeignKey("AreaId")]
        public required Area Area { get; set; }

        [Required]
        [StringLength(10)]
        public required string Status { get; set; }

        [Required]
        public required DateTime InsertDate { get; set; }
    }
}
