namespace Artemis.Backend.Core.DTO.EmployeeManagement
{
    public class EmployeeHistoryDTO
    {
        public int Id { get; set; }
        public string Snapshot { get; set; } = string.Empty;
        public string? Notes { get; set; }
        public string? UserName { get; set; }
        public DateTime InsertDate { get; set; }
    }
}
