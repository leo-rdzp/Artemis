using System.ComponentModel.DataAnnotations;

namespace Artemis.Backend.Core.DTO.Authentication
{
    public class LoginRequestDTO
    {
        [Required(ErrorMessage = "Username is required")]
        public string UserName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; } = string.Empty;

        public string? Action { get; set; }

        public override string ToString()
        {
            return $"Action: {Action}, UserName: {UserName}";
        }
    }
}
