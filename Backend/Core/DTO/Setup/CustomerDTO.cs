using System.Text.Json.Serialization;

namespace Artemis.Backend.Core.DTO.Setup
{
    public class CustomerDTO
    {
        public int Id { get; set; }
        public string? CarrierName { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string? MiddleName { get; set; }
        public string LastName { get; set; } = string.Empty;
        public string Address1 { get; set; } = string.Empty;
        public string? Address2 { get; set; }
        public string City { get; set; } = string.Empty;
        public string State { get; set; } = string.Empty;
        public string ZipCode { get; set; } = string.Empty;
        public string Country { get; set; } = string.Empty;
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public DateTime InsertDate { get; set; }
        public BusinessDTO Business { get; set; } = null!;
        public string? CustomerCode { get; set; }
    }
}
