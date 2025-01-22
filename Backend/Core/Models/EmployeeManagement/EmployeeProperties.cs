using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Artemis.Backend.Core.Models.EmployeeManagement
{
    [Table("EmployeeProperties")]
    public class EmployeeProperties
    {
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public required int Id { get; set; }

        [ForeignKey("EmployeeId")]
        public required Employee Employee { get; set; }

        [Required]
        [StringLength(50)]
        public required string Tag { get; set; }

        [Required]
        [StringLength(15)]
        public required string Type { get; set; }

        [Required]
        public required byte[] Value { get; set; }

        [StringLength(100)]
        public string? Comments { get; set; }

        [Required]
        [StringLength(15)]
        public required string UserName { get; set; }

        [Required]
        public required DateTime InsertDate { get; set; }
    }
}
