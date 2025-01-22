using Artemis.Backend.Core.Models.Setup;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Artemis.Backend.Core.Models.Authentication
{
    [Table("ExternalUser")]
    public class ExternalUser
    {
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public required int Id { get; set; }

        [ForeignKey("PersonId")]
        public required Person Person { get; set; }

        [Required]
        [StringLength(100)]
        public required string CompanyName { get; set; }

        [Required]
        [StringLength(20)]
        public required string Type { get; set; }

        public DateTime? ExpirationDate { get; set; }

        [ForeignKey("BusinessId")]
        public required Business Business { get; set; }
    }
}
