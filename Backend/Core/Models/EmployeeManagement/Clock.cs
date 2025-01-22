using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Artemis.Backend.Core.Models.EmployeeManagement
{
    [Table("Clock")]
    public class Clock
    {
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public required int Id { get; set; }

        [Required]
        [StringLength(255)]
        public required string Description { get; set; }

        [Required]
        [StringLength(20)]
        public required string IpAddress { get; set; }

        [Required]
        [StringLength(20)]
        public required string Type { get; set; }

        [Required]
        [StringLength(20)]
        public required string Status { get; set; }

        [Required]
        public required DateTime UpdateDate { get; set; }

        public ICollection<EmployeeAttendance>? Attendances { get; set; }
    }
}
