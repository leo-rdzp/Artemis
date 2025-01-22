using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Artemis.Backend.Core.Models.Authentication
{
    [Table("User")]
    public class User
    {
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public required int Id { get; set; }

        [ForeignKey("PersonId")]
        public required Person Person { get; set; }

        [Required]
        [StringLength(15)]
        public required string UserName { get; set; }

        [Required]
        [StringLength(255)]
        public required string Password { get; set; }

        [Required]
        [StringLength(20)]
        public required string Status { get; set; }

        [Required]
        [StringLength(20)]
        public required string Type { get; set; }

        [Required]
        public required bool RequireMFA { get; set; }

        public DateTime? PasswordExpirationDate { get; set; }

        public DateTime? LastLoginDate { get; set; }

        [Required]
        public required DateTime InsertDate { get; set; }

        [Required]
        public required DateTime UpdateDate { get; set; }

        public ICollection<UserRole>? UserRoles { get; set; }

        public ICollection<UserProperties>? Properties { get; set; }

        public UserImage? UserImage { get; set; }

        public ICollection<UserAreaMap>? AreaMaps { get; set; }

        public ICollection<AccessHistory>? AccessHistory { get; set; }

        public ICollection<LoginAttempt>? LoginAttempts { get; set; }
    }
}
