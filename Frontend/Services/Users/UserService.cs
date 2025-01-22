using Artemis.Frontend.Models.Setup;
using Artemis.Frontend.Services.Api;
using Artemis.Frontend.Services.Notification;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;

namespace Artemis.Frontend.Services.Users
{
    public class UserService(
        IHttpClientFactory clientFactory,
        IJSRuntime jsRuntime,
        ILogger<UserService> logger,
        AuthenticationStateProvider authenticationStateProvider,
        NotificationService notification) : ApiService(clientFactory, jsRuntime, logger, authenticationStateProvider), IUserService
    {
        private readonly ILogger<UserService> _logger = logger;
        private readonly NotificationService _notification = notification;

        public async Task<IEnumerable<UserDTO>> GetUsers()
        {
            try
            {
                var users = new List<UserDTO>();

                var httpRequest = await CreateRequest<object>(HttpMethod.Get, "user");
                var response = await SendAsync<IEnumerable<UserDTO>>(httpRequest);

                if (response != null)
                {
                    return response.Concat(users);
                }

                _logger.LogWarning("No users found");
                _notification.ShowWarning("No users found");
                return users;
            }
            catch (HttpRequestException ex)
            {
                _notification.ShowError(ex.Message);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching users");
                _notification.ShowError("Failed to load users");
                return [];
            }
        }

        public async Task<UserDTO> GetById(int id)
        {
            try
            {
                var request = await CreateRequest<object>(HttpMethod.Get, $"user/{id}");
                return await SendAsync<UserDTO>(request) ?? new();
            }
            catch (HttpRequestException ex)
            {
                _notification.ShowError(ex.Message);
                throw;
            }
            catch (Exception ex)
            {
                _notification.ShowError($"Error deleting user");
                _logger.LogError(ex, "Error deleting user");
                throw;
            }
        }

        public async Task<UserDTO> Create(UserDTO user)
        {
            try
            {
                var request = await CreateRequest(HttpMethod.Post, "user", user);
                var result = await SendAsync<UserDTO>(request);
                _notification.ShowSuccess("User created successfully");
                return result ?? new();
            }
            catch (HttpRequestException ex)
            {
                _notification.ShowError(ex.Message);
                throw;
            }
            catch
            {
                _notification.ShowError("Failed to create user");
                return new();
            }
        }

        public async Task<UserDTO> Update(UserDTO user)
        {
            try
            {
                // Get existing user to preserve unchanged fields
                var existingUser = await GetById(user.Id);

                // Update only allowed fields
                existingUser.UserName = user.UserName;
                existingUser.Password = user.Password;
                existingUser.Status = user.Status;
                existingUser.Type = user.Type;
                existingUser.UpdateDate = DateTime.UtcNow;

                // Update Person fields
                existingUser.Person.FirstName = user.Person.FirstName;
                existingUser.Person.LastName = user.Person.LastName;
                existingUser.Person.Email = user.Person.Email;
                existingUser.Person.Phone = user.Person.Phone;

                var request = await CreateRequest(HttpMethod.Put, "user", existingUser);
                var response = await SendAsync<UserDTO>(request);
                _notification.ShowSuccess("User updated successfully");
                return response ?? new();
            }
            catch (HttpRequestException ex)
            {
                _notification.ShowError(ex.Message);
                throw;
            }
            catch (Exception ex)
            {
                _notification.ShowError($"Failed to update user: {ex.Message}");
                throw;
            }
        }

        public async Task Delete(int id)
        {
            try
            {
                var request = await CreateRequest<object>(HttpMethod.Delete, $"user/{id}");
                await SendAsync<string>(request);
                _notification.ShowSuccess("User deleted successfully");
            }
            catch (HttpRequestException ex)
            {
                _notification.ShowError(ex.Message);
                throw;
            }
            catch (Exception ex)
            {
                _notification.ShowError($"Error deleting user");
                _logger.LogError(ex, "Error deleting user");
                throw;
            }
        }
    }
}
