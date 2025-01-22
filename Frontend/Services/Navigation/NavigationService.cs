using Artemis.Frontend.Models.Navigation;
using Artemis.Frontend.Services.Api;
using Artemis.Frontend.Services.Notification;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;

namespace Artemis.Frontend.Services.Navigation
{
    public class NavigationService(
        IHttpClientFactory httpClientFactory,
        IJSRuntime jsRuntime,
        ILogger<NavigationService> logger,
        AuthenticationStateProvider authenticationStateProvider,
        NotificationService notificationService) : ApiService(httpClientFactory, jsRuntime, logger, authenticationStateProvider), INavigationService
    {
        private readonly ILogger<NavigationService> _logger = logger;
        private readonly NotificationService _notificationService = notificationService;

        public async Task<IEnumerable<ApplicationGroup>> GetUserMenuAsync()
        {
            try
            {
                var defaultMenuGroups = new List<ApplicationGroup>();

                var httpRequest = await CreateRequest<object>(HttpMethod.Get, "authentication/menu");
                var response = await SendAsync<IEnumerable<ApplicationGroup>>(httpRequest);

                if (response != null)
                {
                    return response.Concat(defaultMenuGroups);
                }

                _logger.LogWarning("No menu items found");
                _notificationService.ShowWarning("Using default menu items");
                return defaultMenuGroups;
            }
            catch (HttpRequestException ex) when (ex.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                _logger.LogWarning("Unauthorized when fetching menu");
                return [];
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching menu");
                _notificationService.ShowError("Failed to load menu items");
                return [];
            }
        }
    }
}
