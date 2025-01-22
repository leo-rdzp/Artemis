using Artemis.Backend.Core.DTO.Setup;

namespace Artemis.Backend.Core.DTO.ProcessControl
{
    public class OperationDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public DateTime InsertDate { get; set; }
        public string UserName { get; set; } = string.Empty;
        public AreaDTO Area { get; set; } = null!;
        public BusinessDTO Business { get; set; } = null!;
        public ICollection<OperationPropertiesDTO>? Properties { get; set; }
    }
}
