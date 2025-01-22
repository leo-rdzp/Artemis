using System.Text.Json.Serialization;

namespace Artemis.Backend.Core.DTO.Devices
{
    public class PreloadPropertiesCreateDTO
    {
        [JsonRequired]
        public int PreloadId { get; set; }

        [JsonRequired]
        public string Tag { get; set; } = string.Empty;

        [JsonRequired]
        public string? Value { get; set; }
    }
}
