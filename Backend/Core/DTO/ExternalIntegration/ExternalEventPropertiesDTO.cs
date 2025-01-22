namespace Artemis.Backend.Core.DTO.ExternalIntegration
{
    public class ExternalEventPropertiesDTO
    {
        public int Id { get; set; }
        public string Tag { get; set; } = string.Empty;
        public string Value { get; set; } = string.Empty;
        public string? Comments { get; set; }
        public DateTime InsertDate { get; set; }
    }
}
