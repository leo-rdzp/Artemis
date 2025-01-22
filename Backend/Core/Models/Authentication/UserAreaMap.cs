using Artemis.Backend.Core.Models.Setup;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Artemis.Backend.Core.Models.Authentication
{
    [Table("UserAreaMap")]
    public class UserAreaMap
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
        public required DateTime InsertDate { get; set; }
    }
}
