using Artemis.Frontend.Services.Notification;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components;
using System.Security.Claims;
using Artemis.Frontend.Models.Authentication;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.JSInterop;

namespace Artemis.Frontend.Services.Authentication
{
    public class BlazorAuthStateProvider(
        IHttpClientFactory httpClientFactory,
        NavigationManager navigationManager,
        NotificationService notificationService,
        IJSRuntime jsRuntime,
        ILogger<BlazorAuthStateProvider> logger) : AuthenticationStateProvider
    {
        private readonly IHttpClientFactory _httpClientFactory = httpClientFactory;
        private readonly NavigationManager _navigationManager = navigationManager;
        private readonly NotificationService _notificationService = notificationService;
        private readonly IJSRuntime _jsRuntime = jsRuntime;
        private readonly ILogger<BlazorAuthStateProvider> _logger = logger;
        private ClaimsPrincipal _currentUser = new(new ClaimsIdentity());
        private bool _initialized;

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            if (_currentUser.Identity?.IsAuthenticated ?? false)
            {
                _logger.LogInformation("Returning existing authenticated user");
                return new AuthenticationState(_currentUser);
            }

            try
            {
                // Only try to get token if we haven't initialized yet
                if (!_initialized)
                {
                    var token = await _jsRuntime.InvokeAsync<string>("localStorageInterop.getItem", "authToken");
                    if (!string.IsNullOrEmpty(token))
                    {
                        var handler = new JwtSecurityTokenHandler();
                        var jwtToken = handler.ReadJwtToken(token);
                        var identity = new ClaimsIdentity(jwtToken.Claims, "jwt");
                        _currentUser = new ClaimsPrincipal(identity);
                        _logger.LogInformation("User authenticated from stored token");
                    }
                    _initialized = true;
                }

                return new AuthenticationState(_currentUser);
            }
            catch (InvalidOperationException)
            {
                _logger.LogInformation("Prerendering detected, returning anonymous user");
            }
            catch (JSException ex)
            {
                _logger.LogError(ex, "JS Interop error in GetAuthenticationStateAsync");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetAuthenticationStateAsync");               
            }

            return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
        }

        public async Task<bool> LoginAsync(string username, string password)
        {
            try
            {
                var client = _httpClientFactory.CreateClient("Artemis");
                var response = await client.PostAsJsonAsync("api/authentication/login", new
                {
                    Action = "login",
                    UserName = username,
                    Password = password
                });

                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogWarning("Login failed for user: {Username} with status: {Status}",
                        username, response.StatusCode);
                    return false;
                }

                var result = await response.Content.ReadFromJsonAsync<AuthenticationResult>();
                if (result?.Token == null)
                {
                    _logger.LogWarning("No token received for user: {Username}", username);
                    return false;
                }

                // Create the claims identity
                var handler = new JwtSecurityTokenHandler();
                var token = handler.ReadJwtToken(result.Token);
                var identity = new ClaimsIdentity(token.Claims, "jwt");
                _currentUser = new ClaimsPrincipal(identity);

                // Store token
                await _jsRuntime.InvokeVoidAsync("localStorageInterop.setItem", "authToken", result.Token);
                _logger.LogInformation("Token stored successfully");

                // Verify the user is now authenticated
                _logger.LogInformation("User authenticated status: {IsAuthenticated}",
                    _currentUser.Identity?.IsAuthenticated);

                // Notify state change
                var authState = new AuthenticationState(_currentUser);
                NotifyAuthenticationStateChanged(Task.FromResult(authState));
                _logger.LogInformation("Authentication state updated for user: {Username}", username);

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Login failed for user: {Username}", username);
                _notificationService.ShowError("Login failed: " + ex.Message);
                return false;
            }
        }

        public async ValueTask Logout()
        {
            try
            {
                _logger.LogInformation("Starting logout process");

                // Reset state first before clearing storage
                _currentUser = new ClaimsPrincipal(new ClaimsIdentity());
                NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(_currentUser)));

                try
                {
                    await _jsRuntime.InvokeVoidAsync("localStorageInterop.removeItem", "authToken");
                    _logger.LogInformation("Auth token removed from storage");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error clearing auth token from storage");
                }

                // Navigate last
                _navigationManager.NavigateTo("/login", true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during logout");
                // Ensure we still get to login page
                _navigationManager.NavigateTo("/login", true);
            }
        }

        public async Task HandleSessionExpired()
        {
            try
            {
                _currentUser = new ClaimsPrincipal(new ClaimsIdentity());
                NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(_currentUser)));

                // Show message and redirect
                _notificationService.ShowWarning("Session expired, please login again");
                await Task.Delay(1000); // Give time for notification
                _navigationManager.NavigateTo("/login", true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error handling session expiration");
                _navigationManager.NavigateTo("/login", true);
            }
        }
    }
}
