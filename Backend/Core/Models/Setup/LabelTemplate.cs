using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Artemis.Backend.Core.Models.Setup
{
    [Table("LabelTemplate")]
    public class LabelTemplate
    {
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public required int Id { get; set; }
        [Required]
        [StringLength(100)]
        public required string Name { get; set; }
        [Required]
        public required byte[] Value { get; set; }
        [Required]
        public required DateTime InsertDate { get; set; }
        [Required]
        [StringLength(15)]
        public required string UserName { get; set; }
        [ForeignKey("BusinessId")]
        public required Business Business { get; set; }
    }
}
