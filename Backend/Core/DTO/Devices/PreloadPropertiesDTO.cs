namespace Artemis.Backend.Core.DTO.Devices
{
    public class PreloadPropertiesDTO
    {
        public int Id { get; set; }
        public string Tag { get; set; } = string.Empty;
        public string? Value { get; set; }
        public DateTime InsertDate { get; set; }
    }
}
