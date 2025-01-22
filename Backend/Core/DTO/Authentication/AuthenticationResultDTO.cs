namespace Artemis.Backend.Core.DTO.Authentication
{
    public class AuthenticationResultDTO
    {
        public UserDTO User { get; set; } = new();

        public string Token { get; set; } = string.Empty;

        public ICollection<ClaimDTO>? Claims { get; set; }
    }
}
