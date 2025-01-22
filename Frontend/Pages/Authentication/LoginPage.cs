using Artemis.Frontend.Models.Authentication;
using Artemis.Frontend.Services.Authentication;
using Artemis.Frontend.Services.Notification;
using Microsoft.AspNetCore.Components;

namespace Artemis.Frontend.Pages.Authentication;

public partial class LoginPage : ComponentBase
{
    [Inject] private BlazorAuthStateProvider AuthService { get; set; } = default!;
    [Inject] private NavigationManager NavigationManager { get; set; } = default!;
    [Inject] private ILogger<LoginPage> Logger { get; set; } = default!;
    [Inject] private NotificationService NotificationService { get; set; } = default!;

    protected LoginRequest LoginModel { get; set; } = new();
    protected bool IsLoading { get; set; }
    protected string ReturnUrl { get; set; } = "/";

    protected override void OnInitialized()
    {
        var uri = new Uri(NavigationManager.Uri);
        var query = System.Web.HttpUtility.ParseQueryString(uri.Query);
        ReturnUrl = query["returnUrl"]?.TrimStart('=') ?? "/";

        // Ensure the returnUrl is relative
        if (Uri.TryCreate(ReturnUrl, UriKind.RelativeOrAbsolute, out var parsedUri)
            && parsedUri.IsAbsoluteUri)
        {
            ReturnUrl = "/";
        }
    }

    protected async Task HandleLogin()
    {
        try
        {
            IsLoading = true;
            StateHasChanged();

            Logger.LogInformation("Attempting login for user: {Username}", LoginModel.UserName);

            if (await AuthService.LoginAsync(LoginModel.UserName, LoginModel.Password))
            {
                // Verify auth state after login
                var authState = await AuthService.GetAuthenticationStateAsync();
                Logger.LogInformation("Auth state after login - IsAuthenticated: {IsAuthenticated}",
                    authState.User.Identity?.IsAuthenticated);

                if (authState.User.Identity?.IsAuthenticated ?? false)
                {
                    Logger.LogInformation("User authenticated, navigating to: {ReturnUrl}", ReturnUrl);
                    NavigationManager.NavigateTo(ReturnUrl ?? "/", true);  // Force reload
                }
                else
                {
                    Logger.LogWarning("Login succeeded but user is not authenticated");
                    NotificationService.ShowError("Authentication error occurred");
                }
                return;
            }

            Logger.LogWarning("Login failed for user: {Username}", LoginModel.UserName);
            NotificationService.ShowError("Invalid username or password");
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Error during login");
            NotificationService.ShowError("An error occurred during login");
        }
        finally
        {
            IsLoading = false;
            StateHasChanged();
        }
    }
}
