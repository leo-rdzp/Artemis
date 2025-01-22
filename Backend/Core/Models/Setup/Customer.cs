using Artemis.Backend.Core.Models.OrderProcessing;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Artemis.Backend.Core.Models.Setup
{
    [Table("Customer")]
    public class Customer
    {
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public required int Id { get; set; }
        [StringLength(20)]
        public string? CarrierName { get; set; }
        [Required]
        [StringLength(30)]
        public required string FirstName { get; set; }
        [StringLength(30)]
        public string? MiddleName { get; set; }
        [Required]
        [StringLength(30)]
        public required string LastName { get; set; }
        [Required]
        [StringLength(40)]
        public required string Address1 { get; set; }
        [StringLength(20)]
        public string? Address2 { get; set; }
        [Required]
        [StringLength(15)]
        public required string City { get; set; }
        [Required]
        [StringLength(15)]
        public required string State { get; set; }
        [Required]
        [StringLength(8)]
        public required string ZipCode { get; set; }
        [Required]
        [StringLength(8)]
        public required string Country { get; set; }
        [StringLength(40)]
        [EmailAddress]
        public string? Email { get; set; }
        [StringLength(15)]
        public string? Phone { get; set; }
        [Required]
        public required DateTime InsertDate { get; set; }
        [Required]
        public required int BusinessId { get; set; }
        [StringLength(20)]
        public string? CustomerCode { get; set; }
        [ForeignKey("BusinessId")]
        public required Business Business { get; set; }
    }
}
