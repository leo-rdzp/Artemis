using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Artemis.Backend.Core.Models.EmployeeManagement
{
    [Table("EmployeeAttendance")]
    public class EmployeeAttendance
    {
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public required int Id { get; set; }

        [ForeignKey("EmployeeId")]
        public required Employee Employee { get; set; }

        public DateTime? AttendanceDate { get; set; }

        [ForeignKey("ClockId")]
        public Clock? Clock { get; set; }

        [Required]
        [StringLength(10)]
        public required string Status { get; set; }

        [Required]
        public required DateTime InsertDate { get; set; }

        [StringLength(15)]
        public string? UserName { get; set; }

        public DateTime? UpdateDate { get; set; }

        [StringLength(100)]
        public string? Comments { get; set; }
    }
}
