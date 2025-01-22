using Artemis.Backend.Core.Models.Authentication;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Artemis.Backend.Core.Models.Notifications
{
    [Table("NotificationAcknowledgment")]
    public class NotificationAcknowledgment
    {
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public required int Id { get; set; }

        [ForeignKey("NotificationId")]
        public required Notification Notification { get; set; }

        [ForeignKey("UserId")]
        public required User User { get; set; }

        [Required]
        public required DateTime AcknowledgmentDate { get; set; }

        [StringLength(255)]
        public string? Comments { get; set; }
    }
}
