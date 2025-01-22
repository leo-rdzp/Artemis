namespace Artemis.Backend.Core.DTO.EmployeeManagement
{
    public class EmployeeAttendanceDTO
    {
        public int Id { get; set; }
        public DateTime? AttendanceDate { get; set; }
        public ClockDTO? Clock { get; set; }
        public string Status { get; set; } = string.Empty;
        public DateTime InsertDate { get; set; }
        public string? UserName { get; set; }
        public DateTime? UpdateDate { get; set; }
        public string? Comments { get; set; }
    }
}
