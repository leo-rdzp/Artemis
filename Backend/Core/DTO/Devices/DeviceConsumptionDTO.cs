using Artemis.Backend.Core.DTO.Materials;

namespace Artemis.Backend.Core.DTO.Devices
{
    public class DeviceConsumptionDTO
    {
        public int Id { get; set; }
        public MaterialDTO Material { get; set; } = null!;
        public string? MaterialSN { get; set; }
        public int Quantity { get; set; }
        public DefectDTO? Defect { get; set; }
        public RepairActionDTO RepairAction { get; set; } = null!;
        public string MatDischarged { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public string ClaimFlag { get; set; } = string.Empty;
        public DateTime InsertDate { get; set; }
        public DateTime UpdateDate { get; set; }
    }
}
