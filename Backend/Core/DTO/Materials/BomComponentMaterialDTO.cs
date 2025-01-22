namespace Artemis.Backend.Core.DTO.Materials
{
    public class BomComponentMaterialDTO
    {
        public int Id { get; set; }
        public int BomComponentId { get; set; }
        public int MaterialId { get; set; }
        public string MaterialName { get; set; } = string.Empty;
        public int Priority { get; set; }
        public string? UseCondition { get; set; }
        public string Status { get; set; } = string.Empty;
        public DateTime InsertDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public string UserName { get; set; } = string.Empty;
    }
}
