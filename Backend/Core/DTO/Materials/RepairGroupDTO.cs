using Artemis.Backend.Core.DTO.Setup;

namespace Artemis.Backend.Core.DTO.Materials
{
    public class RepairGroupDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime InsertDate { get; set; }
        public string UserName { get; set; } = string.Empty;
        public BusinessDTO Business { get; set; } = null!;
    }
}
