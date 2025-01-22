namespace Artemis.Backend.Core.DTO.ExternalIntegration
{
    public class RequestPropertiesDTO
    {
        public int Id { get; set; }
        public string Tag { get; set; } = string.Empty;
        public string Value { get; set; } = string.Empty;
        public DateTime InsertDate { get; set; }
    }
}
