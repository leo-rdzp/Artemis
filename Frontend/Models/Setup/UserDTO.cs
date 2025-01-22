using Artemis.Backend.Core.DTO.Authentication;

namespace Artemis.Frontend.Models.Setup
{
    public class UserDTO
    {
        public int Id { get; set; }
        public PersonDTO Person { get; set; } = new();
        public int PersonId { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public bool RequireMFA { get; set; }
        public DateTime? PasswordExpirationDate { get; set; }
        public DateTime? LastLoginDate { get; set; }
        public DateTime InsertDate { get; set; }
        public DateTime UpdateDate { get; set; }
    }
}
