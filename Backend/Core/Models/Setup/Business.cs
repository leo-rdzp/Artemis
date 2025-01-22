using Artemis.Backend.Core.Models.EmployeeManagement;
using Artemis.Backend.Core.Models.Materials;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Artemis.Backend.Core.Models.Setup
{
    [Table("Business")]
    public class Business
    {
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public required int Id { get; set; }
        [Required]
        [StringLength(25)]
        public required string Name { get; set; }
        [Required]
        [StringLength(4)]
        public required string SapPlant { get; set; }
        [Required]
        [StringLength(100)]
        public required string Icon { get; set; }
        [Required]
        [StringLength(20)]
        public required string Status { get; set; }
        [Required]
        [StringLength(20)]
        public required string Type { get; set; }
        [StringLength(100)]
        public string? BaseUrl { get; set; }
        [Required]
        public required DateTime InsertDate { get; set; }
        [Required]
        public required DateTime UpdateDate { get; set; }
        public ICollection<Product>? Products { get; set; }
        public ICollection<Application>? Applications { get; set; }
        public ICollection<Area>? Areas { get; set; }
    }
}
