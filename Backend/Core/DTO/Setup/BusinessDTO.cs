using System.Text.Json.Serialization;

namespace Artemis.Backend.Core.DTO.Setup
{
    public class BusinessDTO
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? SapPlant { get; set; }
        public string? Icon { get; set; }
        public string Status { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public string? BaseUrl { get; set; }
        public DateTime InsertDate { get; set; }
        public DateTime UpdateDate { get; set; }
    }
}
