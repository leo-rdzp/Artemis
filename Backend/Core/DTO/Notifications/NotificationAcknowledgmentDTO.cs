using Artemis.Backend.Core.DTO.Authentication;

namespace Artemis.Backend.Core.DTO.Notifications
{
    public class NotificationAcknowledgmentDTO
    {
        public int Id { get; set; }
        public NotificationDTO Notification { get; set; } = null!;
        public UserDTO User { get; set; } = null!;
        public DateTime AcknowledgmentDate { get; set; }
        public string? Comments { get; set; }
    }
}
