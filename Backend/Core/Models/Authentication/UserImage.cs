using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Artemis.Backend.Core.Models.Authentication
{
    [Table("UserImage")]
    public class UserImage
    {
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public required int Id { get; set; }

        [ForeignKey("UserId")]
        public required User User { get; set; }

        [Required]
        public required byte[] Image { get; set; }

        [Required]
        public required DateTime InsertDate { get; set; }
    }
}
