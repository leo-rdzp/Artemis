using Artemis.Backend.Core.DTO.Authentication;
using Artemis.Backend.Core.DTO.Setup;

namespace Artemis.Backend.Core.DTO.EmployeeManagement
{
    public class EmployeeDTO
    {
        public int Id { get; set; }
        public PersonDTO Person { get; set; } = null!;
        public string EmployeeNumber { get; set; } = string.Empty;
        public JobTitleDTO JobTitle { get; set; } = null!;
        public AgencyDTO Agency { get; set; } = null!;
        public int? CardNumber { get; set; }
        public DateTime HireDate { get; set; }
        public EmployeeDTO? Supervisor { get; set; }
        public BusinessDTO Business { get; set; } = null!;
        public ICollection<EmployeeDTO>? Subordinates { get; set; }
        public ICollection<EmployeePropertiesDTO>? Properties { get; set; }
        public ICollection<EmployeeHistoryDTO>? History { get; set; }
        public ICollection<EmployeeAttendanceDTO>? Attendances { get; set; }
    }
}
