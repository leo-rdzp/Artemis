namespace Artemis.Backend.Core.DTO.Materials
{
    public class BomUsageRuleDTO
    {
        public int Id { get; set; }
        public int BomComponentId { get; set; }
        public string RuleType { get; set; } = string.Empty;
        public string Condition { get; set; } = string.Empty;
        public string Value { get; set; } = string.Empty;
        public DateTime InsertDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public string UserName { get; set; } = string.Empty;
    }
}
