using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Artemis.Backend.Core.Models.Authentication
{
    [Table("UserProperties")]
    public class UserProperties
    {
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public required int Id { get; set; }

        [ForeignKey("UserId")]
        public required User User { get; set; }

        [Required]
        [StringLength(50)]
        public required string Tag { get; set; }

        [Required]
        [StringLength(160)]
        public required string Value { get; set; }

        [StringLength(100)]
        public string? Comments { get; set; }

        [Required]
        [StringLength(15)]
        public required string UserName { get; set; }

        [Required]
        public required DateTime InsertDate { get; set; }
    }
}
