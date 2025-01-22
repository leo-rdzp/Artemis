using Artemis.Backend.Core.Models.EmployeeManagement;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Artemis.Backend.Core.Models.Authentication
{
    [Table("Person")]
    public class Person
    {
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public required int Id { get; set; }

        [Required]
        [StringLength(25)]
        public required string FirstName { get; set; }

        [Required]
        [StringLength(25)]
        public required string LastName { get; set; }

        [StringLength(100)]
        public required string Email { get; set; }

        [StringLength(15)]
        public string? Phone { get; set; }

        [Required]
        [StringLength(20)]
        public required string Status { get; set; }

        [Required]
        public required DateTime InsertDate { get; set; }

        [Required]
        public required DateTime UpdateDate { get; set; }

        public Employee? Employee { get; set; }

        public ExternalUser? ExternalUser { get; set; }

        public ICollection<User>? Users { get; set; }
    }
}
