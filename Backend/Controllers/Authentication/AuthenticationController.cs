using Artemis.Backend.Core.DTO.Authentication;
using Artemis.Backend.IServices.Base;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Authorization;
using Artemis.Backend.Services.Authentication;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;

namespace Artemis.Backend.Controllers.Authentication;

[ApiController]
[Route("api/[controller]")]
public class AuthenticationController(
    IService<LoginRequestDTO> authenticationService,
    IService<string> menuService,
    IAntiforgery antiforgery,
    ILogger<AuthenticationController> logger) : ControllerBase
{
    private readonly IService<LoginRequestDTO> _authenticationService = authenticationService;
    private readonly IService<string> _menuService = menuService;    
    private readonly IAntiforgery _antiforgery = antiforgery;
    private readonly ILogger<AuthenticationController> _logger = logger;

    [HttpGet("antiforgery")]
    [AllowAnonymous]
    public IActionResult GetAntiForgeryTokens()
    {
        var tokens = _antiforgery.GetAndStoreTokens(HttpContext);
        return Ok(new { token = tokens.RequestToken });
    }

    [HttpGet("token")]
    public async Task<IActionResult> GetToken()
    {
        try
        {
            var userName = User.Identity?.Name;
            var tokenRequest = new LoginRequestDTO
            {
                Action = "token",
                UserName = userName ?? string.Empty
            };

            var result = await _authenticationService.ExecuteAsync(tokenRequest);

            if (result.ResultStatus.IsPassed)
            {
                return Ok(result.ResultData);
            }

            return NotFound(result.ResultStatus.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting token");
            return StatusCode(500, "An error occurred while getting token");
        }
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequestDTO loginRequest)
    {
        try
        {
            var result = await _authenticationService.ExecuteAsync(loginRequest);
            if (!result.ResultStatus.IsPassed)
            {
                return BadRequest(result.ResultStatus.Message);
            }

            if (result.ResultData is not AuthenticationResultDTO authenticationResult)
            {
                return BadRequest("Invalid user data received");
            }

            var claims = new List<Claim>
            {
                new(ClaimTypes.Name, authenticationResult.User.UserName),
                new(ClaimTypes.NameIdentifier, authenticationResult.User.Id.ToString()),
                new(ClaimTypes.GivenName, authenticationResult.User.Person.FirstName)
            };

            if (authenticationResult.User.Roles != null)
            {
                foreach (var role in authenticationResult.User.Roles)
                {
                    claims.Add(new Claim(ClaimTypes.Role, role.Name ?? string.Empty));
                }
            }

            var authProperties = new AuthenticationProperties
            {
                IsPersistent = true,
                ExpiresUtc = DateTimeOffset.UtcNow.AddHours(8),
                AllowRefresh = true,
                RedirectUri = "/"
            };

            var identity = new ClaimsIdentity(claims, "Artemis");
            var principal = new ClaimsPrincipal(identity);

            try
            {
                // Explicitly set authentication cookie
                await HttpContext.SignInAsync(
                    ArtemisAuthenticationDefaults.AuthenticationScheme,
                    principal,
                    new AuthenticationProperties
                    {
                        IsPersistent = true,
                        ExpiresUtc = DateTimeOffset.UtcNow.AddHours(8),
                        AllowRefresh = true,
                        IssuedUtc = DateTimeOffset.UtcNow,
                    });

                // Set HttpContext.User explicitly
                HttpContext.User = principal;

                // Verify cookie was set
                //var cookieValue = HttpContext.Response.Headers.SetCookie;
                //_logger.LogInformation("Cookie header set: {cookie}", cookieValue.ToString() ?? string.Empty);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during SignInAsync");
                throw;
            }

            var claimDtos = claims.Select(c => new ClaimDTO { Type = c.Type, Value = c.Value }).ToList();
            authenticationResult.Claims = claimDtos;

            return Ok(authenticationResult);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during login");
            return StatusCode(500, "An error occurred during login");
        }
    }

    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        try
        {
            var userName = User.Identity?.Name;
            var logoutRequest = new LoginRequestDTO
            {
                Action = "logout",
                UserName = userName ?? string.Empty
            };

            var result = await _authenticationService.ExecuteAsync(logoutRequest);

            if (result.ResultStatus.IsPassed)
            {                
                return Ok(result.ResultStatus.Message);
            }

            return BadRequest(result.ResultStatus.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during logout");
            return StatusCode(500, "An error occurred during logout");
        }
    }

    [HttpGet("menu")]
    public async Task<IActionResult> GetUserMenu()
    {
        try
        {
            string? userName = User.Identity?.Name;
            if (string.IsNullOrEmpty(userName))
            {
                return Unauthorized();
            }
            
            var result = await _menuService.ExecuteAsync(userName);

            if (result.ResultStatus.IsPassed)
            {
                return Ok(result.ResultData);
            }

            return NotFound(result.ResultStatus.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting user menu");
            return StatusCode(500, "An error occurred while getting user menu");
        }
    }
}