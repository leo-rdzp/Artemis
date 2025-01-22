namespace Artemis.Frontend.Models.Authentication
{
    public class AuthenticationResult
    {
        public UserInfo User { get; set; } = null!;
        public string Token { get; set; } = string.Empty;
        public IEnumerable<ClaimInfo> Claims { get; set; } = [];
    }
}
