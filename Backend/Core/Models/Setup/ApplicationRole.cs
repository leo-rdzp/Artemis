using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Artemis.Backend.Core.Models.Authentication;

namespace Artemis.Backend.Core.Models.Setup
{
    [Table("ApplicationRole")]
    public class ApplicationRole
    {
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public required int Id { get; set; }

        [ForeignKey("RoleId")]
        public required Role Role { get; set; }

        [ForeignKey("ApplicationId")]
        public required Application Application { get; set; }        

        [Required]
        public required DateTime InsertDate { get; set; }

        [StringLength(50)]
        public required string AssignedBy { get; set; }
    }
}
