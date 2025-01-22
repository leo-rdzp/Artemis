using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Artemis.Backend.Core.Models.Authentication
{
    [Table("UserRole")]
    public class UserRole
    {
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public required int Id { get; set; }

        [ForeignKey("UserId")]
        public required User User { get; set; }

        [ForeignKey("RoleId")]
        public required Role Role { get; set; }

        [StringLength(50)]
        public required string AssignedBy { get; set; }

        [Required]
        public required DateTime AssignedDate { get; set; }

        public DateTime? ExpirationDate { get; set; }
    }
}
