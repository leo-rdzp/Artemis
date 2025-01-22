namespace Artemis.Backend.Core.DTO.Materials
{
    public class BomRevisionHistoryDTO
    {
        public int Id { get; set; }
        public int BomHeaderId { get; set; }
        public string RevisionNumber { get; set; } = string.Empty;
        public string ChangeType { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int ChangedBy { get; set; }
        public string ChangedByUserName { get; set; } = string.Empty;
        public DateTime ChangeDate { get; set; }
    }
}
