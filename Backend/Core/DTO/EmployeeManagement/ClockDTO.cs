namespace Artemis.Backend.Core.DTO.EmployeeManagement
{
    public class ClockDTO
    {
        public int Id { get; set; }
        public string Description { get; set; } = string.Empty;
        public string IpAddress { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public DateTime UpdateDate { get; set; }
        public ICollection<EmployeeAttendanceDTO>? Attendances { get; set; }
    }
}
