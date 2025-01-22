namespace Artemis.Backend.Core.DTO.EmployeeManagement
{
    public class JobTitleDTO
    {
        public int Id { get; set; }
        public string Code { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public DepartmentDTO Department { get; set; } = null!;
        public string Status { get; set; } = string.Empty;
        public string SupervisoryRole { get; set; } = string.Empty;
        public DateTime InsertDate { get; set; }
        public string UserName { get; set; } = string.Empty;
        public ICollection<EmployeeDTO>? Employees { get; set; }
    }
}
