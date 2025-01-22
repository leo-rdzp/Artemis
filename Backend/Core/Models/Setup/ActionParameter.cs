using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Artemis.Backend.Core.Models.Setup
{
    [Table("ActionParameter")]
    public class ActionParameter
    {
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public required int Id { get; set; }

        [Required]
        [StringLength(100)]
        public required string ActionName { get; set; }

        [Required]
        [StringLength(100)]
        public required string ParameterName { get; set; }

        [Required]
        [StringLength(255)]
        public required string Value { get; set; }

        [StringLength(100)]
        public string? Comments { get; set; }

        [Required]
        [StringLength(15)]
        public required string UserName { get; set; }

        [Required]
        public DateTime InsertDate { get; set; }

        [ForeignKey("BusinessId")]
        public required Business Business { get; set; }
    }
}