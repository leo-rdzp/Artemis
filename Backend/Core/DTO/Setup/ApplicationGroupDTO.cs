using System.Text.Json.Serialization;

namespace Artemis.Backend.Core.DTO.Setup
{
    public class ApplicationGroupDTO
    {
        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;

        [JsonPropertyName("applications")]
        public List<ApplicationDTO>? Applications { get; set; }
    }
}
