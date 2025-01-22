using Artemis.Backend.Core.DTO.Setup;

namespace Artemis.Backend.Core.DTO.ExternalIntegration
{
    public class RequestDTO
    {
        public int Id { get; set; }
        public string Number { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public BusinessDTO Business { get; set; } = null!;
        public DateTime InsertDate { get; set; }
        public ICollection<RequestPropertiesDTO>? Properties { get; set; }
    }
}
