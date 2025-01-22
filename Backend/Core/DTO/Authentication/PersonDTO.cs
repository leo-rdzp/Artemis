using System.Text.Json.Serialization;

namespace Artemis.Backend.Core.DTO.Authentication
{
    public class PersonDTO
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string Status { get; set; } = string.Empty;
        public DateTime InsertDate { get; set; }
        public DateTime UpdateDate { get; set; }

        [JsonIgnore]
        public ICollection<ExternalUserDTO>? ExternalUsers { get; set; }
        [JsonIgnore]
        public ICollection<UserDTO>? Users { get; set; }
    }
}
