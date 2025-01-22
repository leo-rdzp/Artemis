using Artemis.Backend.Core.DTO.Setup;

namespace Artemis.Backend.Core.DTO.Materials
{
    public class DefectDTO
    {
        public int Id { get; set; }
        public string Code { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public RepairGroupDTO RepairGroup { get; set; } = null!;
        public string? Status { get; set; }
        public BusinessDTO Business { get; set; } = null!;
        public int? Precedence { get; set; }
        public DateTime InsertDate { get; set; }
        public string UserName { get; set; } = string.Empty;
    }
}
