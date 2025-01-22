using System.Text.Json.Serialization;

namespace Artemis.Backend.Core.DTO.Setup
{
    public class ActionParameterDTO
    {
        public int Id { get; set; }
        public string? ActionName { get; set; }
        public string? ParameterName { get; set; }
        public string? Value { get; set; }
        public string? Comments { get; set; }
        public string UserName { get; set; } = string.Empty;
        public DateTime InsertDate { get; set; }
        public BusinessDTO Business { get; set; } = null!;
    }
}
