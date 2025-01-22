using Microsoft.JSInterop;
using System.Text.Json.Serialization;
using System.Text.Json;
using Microsoft.Azure.ServiceBus;
using System.Net.Http.Headers;
using Artemis.Frontend.Services.Authentication;
using Microsoft.AspNetCore.Components.Authorization;

namespace Artemis.Frontend.Services.Api
{
    public class ApiService(
        IHttpClientFactory httpClientFactory, 
        IJSRuntime jsRuntime,
        ILogger logger,
        AuthenticationStateProvider authStateProvider
        )
    {
        private readonly HttpClient _httpClient = httpClientFactory.CreateClient("Artemis");
        private readonly IJSRuntime _jsRuntime = jsRuntime;
        private readonly ILogger _logger = logger;
        private readonly AuthenticationStateProvider _authStateProvider = authStateProvider;

        protected async Task<string> GetAntiForgeryToken()
        {
            try
            {
                return await _jsRuntime.InvokeAsync<string>("getAntiForgeryToken");
            }
            catch
            {
                return string.Empty;
            }
        }

        protected async Task<HttpRequestMessage> CreateRequest<TContent>(
            HttpMethod method,
            string endpoint,
            TContent? content = default)
        {
            var request = new HttpRequestMessage(method, $"api/{endpoint}");

            if (content != null)
            {
                request.Content = JsonContent.Create(content);
            }

            // Add auth token if available
            try
            {
                // Check if we're prerendering or can access JSRuntime
                if (_jsRuntime is not IJSInProcessRuntime)
                {
                    // We're prerendering, return request without auth
                    return request;
                }

                var token = await _jsRuntime.InvokeAsync<string>("localStorageInterop.getItem", "authToken");
                if (!string.IsNullOrEmpty(token))
                {
                    request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
                }
            }
            catch (InvalidOperationException)
            {
                // Prerendering, continue without token
            }
            catch (JSException ex)
            {
                // Log but continue
                _logger.LogError(ex, "Error getting auth token: ");
            }

            // Add antiforgery token
            var antiforgeryToken = await GetAntiForgeryToken();
            if (!string.IsNullOrEmpty(antiforgeryToken))
            {
                request.Headers.Add("RequestVerificationToken", antiforgeryToken);
            }            

            return request;
        }

        protected async Task<TResponse?> SendAsync<TResponse>(HttpRequestMessage request)
        {
            var response = await _httpClient.SendAsync(request);
            var content = await response.Content.ReadAsStringAsync();
            var contentType = response.Content.Headers.ContentType?.MediaType;

            // Check if we got HTML instead of JSON (likely a redirect to login page)
            if (contentType?.Contains("text/html") == true || content.StartsWith("<"))
            {
                _logger.LogWarning("Received HTML response instead of JSON, session likely expired");
                await HandleUnauthorized();
                throw new UnauthorizedException("Session expired");
            }

            // Handle unauthorized/session expired
            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                _logger.LogError("Unauthorized response received");
                await HandleUnauthorized();
                throw new UnauthorizedException("Session expired, please login again");
            }

            if (!response.IsSuccessStatusCode)
            {
                if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    await HandleUnauthorized();
                    throw new UnauthorizedException("Session expired");
                }
                throw new HttpRequestException(content);               
            }

            if (typeof(TResponse) == typeof(string))
            {
                return (TResponse)(object)content;
            }

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
            };

            try
            {
                var jsonDocument = JsonDocument.Parse(content);
                var rootElement = jsonDocument.RootElement;

                // Handle returned an Array
                if (rootElement.ValueKind == JsonValueKind.Array)
                {
                    return JsonSerializer.Deserialize<TResponse>(rootElement, options);
                }

                // Handle items wrapper
                if (rootElement.TryGetProperty("items", out JsonElement itemsElement))
                {
                    if (typeof(TResponse).IsGenericType &&
                        typeof(TResponse).GetGenericTypeDefinition() == typeof(IEnumerable<>))
                    {
                        return JsonSerializer.Deserialize<TResponse>(itemsElement.GetRawText(), options);
                    }
                }

                // Handle collections within objects (like claims in login response)
                return JsonSerializer.Deserialize<TResponse>(content, options);
            }
            catch (JsonException ex)
            {
                _logger.LogError(ex, "Deserialization Error:");
                throw;
            }
        }
        private async Task HandleUnauthorized()
        {
            try
            {
                // Clear the token
                await _jsRuntime.InvokeVoidAsync("localStorageInterop.removeItem", "authToken");

                // Notify auth state provider
                if (_authStateProvider is BlazorAuthStateProvider authProvider)
                {
                    await authProvider.HandleSessionExpired();
                }
            }
            catch
            {
                // Ignore errors during cleanup
            }
        }
    }
}
