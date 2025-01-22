using Artemis.Backend.Core.DTO.Authentication;

namespace Artemis.Backend.Core.DTO.Notifications
{
    public class NotificationDTO
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public UserDTO User { get; set; } = null!;
        public DateTime InsertDate { get; set; }
        public ICollection<NotificationAcknowledgmentDTO>? Acknowledgments { get; set; }
    }
}
