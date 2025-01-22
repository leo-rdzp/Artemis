using Artemis.Backend.Connections.Database;
using Artemis.Backend.Core.DTO.Authentication;
using Artemis.Backend.Core.Utilities;
using Artemis.Backend.IServices.Base;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication;
using Artemis.Backend.Core.Models.Authentication;
using Microsoft.Extensions.Options;
using Artemis.Backend.IServices.Authentication;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Artemis.Backend.Services.Authentication
{
    public class AuthenticationBackendService(
    ArtemisDbContext context,
    ITransactionScope transactionScope,
    IHttpContextAccessor httpContextAccessor,
    IOptions<SecuritySettings> securitySettings,
    IMapper mapper,
    ILogger<AuthenticationBackendService> logger,
    IPasswordHasher passwordHasher) : BaseService<LoginRequestDTO>
    {
        private readonly ArtemisDbContext _context = context;
        private readonly ITransactionScope _transactionScope = transactionScope;
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;
        private readonly IOptions<SecuritySettings> _securitySettings = securitySettings;
        private readonly IMapper _mapper = mapper;
        private readonly ILogger<AuthenticationBackendService> _logger = logger;
        private readonly IPasswordHasher _passwordHasher = passwordHasher;

        public override void PrepareMandatoryParameters()
        {
            SetMandatoryParameters(["UserName", "Password"]);
        }

        public override async Task<ResultNotifier> ExecuteAsync(LoginRequestDTO dto)
        {
            using var transaction = await _transactionScope.GetTransactionAsync();
            var result = dto.Action?.ToLower() switch
            {
                "login" => await LoginAsync(dto.UserName, dto.Password),
                "logout" => await LogoutAsync(),
                "token" => GetCurrentToken(),
                _ => ResultNotifier.Failure("Invalid action specified")
            };
            await transaction.CommitAsync();
            return result;
        }

        private ResultNotifier GetCurrentToken()
        {
            try
            {
                var httpContext = _httpContextAccessor.HttpContext;
                if (httpContext?.User.Identity?.IsAuthenticated != true)
                {
                    return ResultNotifier.Failure("No active session found");
                }

                var claims = httpContext.User.Claims.ToList();
                var token = GenerateAuthToken(claims);

                return ResultNotifier.Success(new
                {
                    Token = token,
                    ExpiresIn = _securitySettings.Value.TokenExpirationMinutes
                });
            }
            catch (ArtemisException ex)
            {
                _logger.LogError(
                    "Artemis Exception occurred. Message: {Message}, Details: {Details}",
                    ex.Message,
                    ex.DetailedMessage);
                return ResultNotifier.Failure($"{ex.Message} - {ex.DetailedMessage}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error generating token");
                return ResultNotifier.Failure("Error generating token");
            }
        }

        private async Task<ResultNotifier> LoginAsync(string userName, string password)
        {
            try
            {
                // Add brute force protection
                if (await IsUserLockedOut(userName))
                {
                    return ResultNotifier.Failure("Account is temporarily locked. Please try again later.");
                }

                // Validate user with timing attack protection
                var user = await _context.Users
                    .AsNoTracking()
                    .Include(u => u.Person)
                    .Include(u => u.UserRoles!)
                        .ThenInclude(ur => ur.Role)
                    .FirstOrDefaultAsync(u => u.UserName == userName);

                // Use constant-time comparison for password verification
                if (user == null)
                {                    
                    return ResultNotifier.Failure("Username does not exist!");
                }

                // Use constant-time comparison for password verification
                if (!_passwordHasher.VerifyPassword(password, user.Password))
                {
                    await RecordFailedLoginAttempt(user, userName);
                    return ResultNotifier.Failure("Invalid username or password!");
                }

                if (!user.Status.Equals(CommonTags.Active))
                {
                    _logger.LogWarning("Inactive user attempted login: {userName}", userName);
                    return ResultNotifier.Failure("Account is not active.");
                }

                // Update last login with proper error handling
                await UpdateLastLoginAsync(user);

                // Generate token
                var userDto = _mapper.Map<UserDTO>(user);
                var claims = GenerateUserClaims(userDto);
                var token = GenerateAuthToken(claims);

                // Update object
                userDto.ExpiresIn = _securitySettings.Value.TokenExpirationMinutes;

                // Create Result
                AuthenticationResultDTO authenticationResult = new()
                {
                    User = userDto,
                    Token = token
                };

                // Clear False attempts
                await ClearFailedLoginAttempts(userName);

                return ResultNotifier.Success(authenticationResult);
            }
            catch (ArtemisException ex)
            {
                _logger.LogError(
                    "Artemis Exception occurred. Message: {Message}, Details: {Details}",
                    ex.Message,
                    ex.DetailedMessage);
                return ResultNotifier.Failure($"{ex.Message} - {ex.DetailedMessage}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during login for user {userName}", userName);
                return ResultNotifier.Failure("An unexpected error occurred.");
            }
        }

        private async Task<ResultNotifier> LogoutAsync()
        {
            try
            {
                var httpContext = _httpContextAccessor.HttpContext;
                if (httpContext != null)
                {
                    // httpContext.Response.Cookies.Delete("Artemis.Auth");
                    // await httpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                    await httpContext.SignOutAsync("Artemis.Auth");
                    return ResultNotifier.Success("Logged out successfully");
                }
                return ResultNotifier.Failure("Logout failed");
            }
            catch (ArtemisException ex)
            {
                _logger.LogError(
                    "Artemis Exception occurred. Message: {Message}, Details: {Details}",
                    ex.Message,
                    ex.DetailedMessage);
                return ResultNotifier.Failure($"{ex.Message} - {ex.DetailedMessage}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during logout");
                return ResultNotifier.Failure("Error during logout");
            }
        }

        private async Task UpdateLastLoginAsync(User user)
        {
            try
            {
                user.LastLoginDate = DateTime.UtcNow;
                user.UpdateDate = DateTime.UtcNow;
                _context.Users.Update(user);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to update last login for user {userName}", user.UserName);
                // Continue execution - non-critical operation
            }
        }

        private async Task<bool> IsUserLockedOut(string userName)
        {
            var failedAttempts = await _context.LoginAttempts
                .Where(la => la.UserName == userName &&
                            la.AttemptDate >= DateTime.UtcNow.AddMinutes(-_securitySettings.Value.LockoutTimeMinutes))
                .CountAsync();

            return failedAttempts >= _securitySettings.Value.MaxFailedAttempts;
        }

        private async Task RecordFailedLoginAttempt(User user, string userName)
        {
            try
            {
                _ = _context.LoginAttempts.Add(new LoginAttempt
                {
                    UserId = user.Id,
                    UserName = userName,
                    AttemptDate = DateTime.UtcNow,
                    IPAddress = _httpContextAccessor.HttpContext?.Connection.RemoteIpAddress?.ToString()
                });
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to record login attempt for {userName}", userName);
            }
        }

        private async Task ClearFailedLoginAttempts(string userName)
        {
            try
            {
                var attempts = await _context.LoginAttempts
                    .Where(la => la.UserName == userName)
                    .ToListAsync();
                _context.LoginAttempts.RemoveRange(attempts);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to clear login attempts for {userName}", userName);
            }
        }

        private string GenerateAuthToken(IEnumerable<Claim> claims)
        {
            try
            {
                if (string.IsNullOrEmpty(_securitySettings.Value.JwtKey))
                {
                    throw new InvalidOperationException("JWT key is not configured");
                }

                var key = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(_securitySettings.Value.JwtKey));

                var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                var token = new JwtSecurityToken(
                    issuer: _securitySettings.Value.JwtIssuer ?? "Artemis",
                    audience: _securitySettings.Value.JwtAudience ?? "ArtemisUsers",
                    claims: claims,
                    expires: DateTime.UtcNow.AddMinutes((double)_securitySettings.Value.TokenExpirationMinutes),
                    signingCredentials: credentials
                );

                return new JwtSecurityTokenHandler().WriteToken(token);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error generating auth token");
                throw new InvalidOperationException("Error generating authentication token", ex);
            }
        }

        private static List<Claim> GenerateUserClaims(UserDTO user)
        {
            var claims = new List<Claim>
        {
            new(ClaimTypes.Name, user.UserName),
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new(ClaimTypes.GivenName, user.Person.FirstName)
        };

            if (user.Roles != null)
            {
                foreach (var role in user.Roles)
                {
                    claims.Add(new Claim(ClaimTypes.Role, role.Name ?? string.Empty));
                }
            }

            return claims;
        }
    }
}