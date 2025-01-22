using Artemis.Backend.Core.Models.Authentication;
using Artemis.Backend.Core.Models.Setup;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Artemis.Backend.Core.Models.EmployeeManagement
{
    [Table("Employee")]
    public class Employee
    {
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public required int Id { get; set; }

        [ForeignKey("PersonId")]
        public required Person Person { get; set; }

        [Required]
        [StringLength(15)]
        public required string EmployeeNumber { get; set; }

        [ForeignKey("JobTitleId")]
        public required JobTitle JobTitle { get; set; }

        [ForeignKey("AgencyId")]
        public required Agency Agency { get; set; }

        public int? CardNumber { get; set; }

        [Required]
        public required DateTime HireDate { get; set; }

        [ForeignKey("SupervisorId")]
        public Employee? Supervisor { get; set; }

        [ForeignKey("BusinessId")]
        public required Business Business { get; set; }

        public ICollection<Employee>? Subordinates { get; set; }

        public ICollection<EmployeeProperties>? Properties { get; set; }

        public ICollection<EmployeeHistory>? History { get; set; }

        public ICollection<EmployeeAttendance>? Attendances { get; set; }
    }
}
