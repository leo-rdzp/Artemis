using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Artemis.Backend.Core.Models.EmployeeManagement
{
    [Table("EmployeeHistory")]
    public class EmployeeHistory
    {
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public required int Id { get; set; }

        [ForeignKey("EmployeeId")]
        public required Employee Employee { get; set; }

        [Required]
        [StringLength(25)]
        public required string Snapshot { get; set; }

        [StringLength(255)]
        public string? Notes { get; set; }

        [StringLength(15)]
        public string? UserName { get; set; }

        [Required]
        public required DateTime InsertDate { get; set; }
    }
}
