using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Artemis.Backend.Core.Models.EmployeeManagement
{
    [Table("Department")]
    public class Department
    {
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public required int Id { get; set; }

        [Required]
        [StringLength(15)]
        public required string Code { get; set; }

        [Required]
        [StringLength(25)]
        public required string Name { get; set; }

        [Required]
        [StringLength(50)]
        public required string Description { get; set; }

        [Required]
        [StringLength(10)]
        public required string Status { get; set; }

        [Required]
        public required DateTime InsertDate { get; set; }

        [Required]
        [StringLength(15)]
        public required string UserName { get; set; }

        public ICollection<JobTitle>? JobTitles { get; set; }
    }
}
