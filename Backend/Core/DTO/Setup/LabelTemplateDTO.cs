using System.Text.Json.Serialization;

namespace Artemis.Backend.Core.DTO.Setup
{
    public class LabelTemplateDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public byte[] Value { get; set; } = null!;
        public DateTime InsertDate { get; set; }
        public string UserName { get; set; } = string.Empty;
        public BusinessDTO Business { get; set; } = null!;
    }
}
