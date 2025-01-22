using Artemis.Backend.Core.DTO.Setup;

namespace Artemis.Backend.Core.DTO.ExternalIntegration
{
    public class ExternalEventDTO
    {
        public int Id { get; set; }
        public string Direction { get; set; } = string.Empty;
        public string InterfaceType { get; set; } = string.Empty;
        public string Event { get; set; } = string.Empty;
        public string Process { get; set; } = string.Empty;
        public string ServiceName { get; set; } = string.Empty;
        public string CommunicationType { get; set; } = string.Empty;
        public DateTime EventDateRequest { get; set; }
        public DateTime? EventDateResponse { get; set; }
        public string EventStatus { get; set; } = string.Empty;
        public int TryCounter { get; set; }
        public DateTime UpdateDate { get; set; }
        public string? EventResponseCode { get; set; }
        public BusinessDTO Business { get; set; } = null!;
        public ICollection<ExternalEventPropertiesDTO>? Properties { get; set; }
    }
}
