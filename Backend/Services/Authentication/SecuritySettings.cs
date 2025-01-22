namespace Artemis.Backend.Services.Authentication
{
    public class SecuritySettings
    {
        public int MaxFailedAttempts { get; set; } = 5;
        public int LockoutTimeMinutes { get; set; } = 15;
        public int JwtExpiryInDays { get; set; } = 1;
        public int TokenExpirationMinutes { get; set; } = 480;
        public TimeSpan? CookieLifetime { get; set; } = TimeSpan.FromHours(8);
        public string? JwtIssuer { get; internal set; } = string.Empty; 
        public string? JwtAudience { get; internal set; } = string.Empty;
        public string? JwtKey { get; internal set; } = string.Empty;
    }
}
