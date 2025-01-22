using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Artemis.Backend.Core.Models.Authentication
{
    [Table("LoginAttempt")]
    public class LoginAttempt
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int UserId { get; set; }

        [ForeignKey("UserId")]
        public User? User { get; set; }

        [Required]
        [StringLength(15)]
        public string UserName { get; set; } = string.Empty;

        [Required]
        public DateTime AttemptDate { get; set; }

        [StringLength(50)]
        public string? IPAddress { get; set; }
    }
}
