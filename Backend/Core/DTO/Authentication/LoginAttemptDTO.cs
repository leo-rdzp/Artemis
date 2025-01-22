namespace Artemis.Backend.Core.DTO.Authentication
{
    public class LoginAttemptDTO
    {
        public int Id { get; set; }
        public UserDTO User { get; set; } = null!;
        public string UserName { get; set; } = string.Empty;
        public DateTime AttemptDate { get; set; }
        public string? IPAddress { get; set; }
    }
}
