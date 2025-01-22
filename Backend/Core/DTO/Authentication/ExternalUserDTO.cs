using Artemis.Backend.Core.DTO.Setup;

namespace Artemis.Backend.Core.DTO.Authentication
{
    public class ExternalUserDTO
    {
        public int Id { get; set; }
        public PersonDTO Person { get; set; } = null!;
        public string CompanyName { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public DateTime? ExpirationDate { get; set; }
        public BusinessDTO Business { get; set; } = null!;
    }
}
