namespace Artemis.Backend.Core.DTO.OrderProcessing
{
    public class OrderLinePropertiesDTO
    {
        public int Id { get; set; }
        public string Tag { get; set; } = string.Empty;
        public string Value { get; set; } = string.Empty;
        public string? Comments { get; set; }
        public DateTime InsertDate { get; set; }
    }
}
