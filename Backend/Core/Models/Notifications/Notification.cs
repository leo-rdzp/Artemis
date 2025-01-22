using Artemis.Backend.Core.Models.Authentication;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Artemis.Backend.Core.Models.Notifications
{
    [Table("Notification")]
    public class Notification
    {
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public required int Id { get; set; }

        [Required]
        [StringLength(100)]
        public required string Title { get; set; }

        [Required]
        public required string Message { get; set; }

        [Required]
        [StringLength(15)]
        public required string Status { get; set; }

        [Required]
        [StringLength(20)]
        public required string Type { get; set; }

        [ForeignKey("UserId")]
        public required User User { get; set; }

        [Required]
        public required DateTime InsertDate { get; set; }

        public ICollection<NotificationAcknowledgment>? Acknowledgments { get; set; }
    }
}
