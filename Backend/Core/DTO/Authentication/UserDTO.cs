using Artemis.Backend.Core.DTO.Setup;
using System.ComponentModel.DataAnnotations.Schema;

namespace Artemis.Backend.Core.DTO.Authentication
{
    public class UserDTO
    {
        public int Id { get; set; }

        public PersonDTO Person { get; set; } = new();

        public int PersonId { get; set; }

        public string UserName { get; set; } = string.Empty;

        [NotMapped]
        public string Password { get; set; } = string.Empty;

        public int ExpiresIn { get; set; } = 15;

        public string Status { get; set; } = string.Empty;

        public string Type { get; set; } = string.Empty;

        public bool RequireMFA { get; set; }

        public DateTime? PasswordExpirationDate { get; set; }
        public DateTime? LastLoginDate { get; set; }
        public DateTime InsertDate { get; set; }
        public DateTime UpdateDate { get; set; }

        public ICollection<RoleDTO>? Roles { get; set; }
        public ICollection<UserPropertiesDTO>? Properties { get; set; }
        public UserImageDTO? UserImageDTO { get; set; }
        public ICollection<AreaDTO>? Areas { get; set; }
        public ICollection<AccessHistoryDTO>? AccessHistory { get; set; }
        public ICollection<LoginAttemptDTO>? LoginAttempts { get; set; }
    }
}
