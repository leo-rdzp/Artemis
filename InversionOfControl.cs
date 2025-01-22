using Artemis.Backend.Connections.Database;
using Artemis.Backend.Core.DTO.Authentication;
using Artemis.Backend.IServices.Authentication;
using Artemis.Backend.IServices.Base;
using Artemis.Backend.Services.Authentication;
using Artemis.Backend.Services.UserManagement;
using Artemis.Backend.Services.PersonManagement;
using Artemis.Backend.Core.Utilities;
using Artemis.Backend.Services.RoleManagement;
using Artemis.Backend.Services.BusinessManagement;
using Artemis.Backend.Services.ApplicationManagement;
using Artemis.Frontend.Services.Authentication;
using Artemis.Frontend.Services.Notification;
using Artemis.Frontend.Services.Navigation;
using Artemis.Frontend.Services.Users;
using Microsoft.AspNetCore.Components.Authorization;

namespace Artemis
{
    public static class InversionOfControl
    {
        public static IServiceCollection AddServiceRegistration(IServiceCollection services)
        {
            /* 
             * Frontend Services 
            */
            services.AddScoped<NotificationService>();
            services.AddScoped<AuthenticationStateProvider, BlazorAuthStateProvider>();
            services.AddAuthorizationCore();

            services.AddScoped<BlazorAuthStateProvider>();
            services.AddScoped<MenuStateService>();
            services.AddScoped<ErrorService>();            
            services.AddRazorComponents()
                .AddInteractiveServerComponents();

            services.AddScoped<INavigationService, NavigationService>();
            services.AddScoped<IUserService, UserService>();

            /* 
             * Backend Services 
            */
            // Core Services
            services.AddScoped<ITransactionScope, TransactionScope>();
            services.AddScoped<IPasswordHasher, PasswordHasher>();

            // Authentication & Authorization
            services.AddScoped<IService<LoginRequestDTO>, AuthenticationBackendService>();
            services.AddScoped<IService<string>, MenuService>();
            services.AddCascadingAuthenticationState();

            // Add tools as needed
            services.AddScoped<ItemListTools, ItemListTools>();
            services.AddScoped<MappingTools, MappingTools>();
            services.AddScoped<EmailTools, EmailTools>();
            services.AddScoped<EncryptionTools, EncryptionTools>();

            // User Services
            services.AddScoped<GetUsersService>();
            services.AddScoped<GetUserService>();
            services.AddScoped<CreateUserService>();
            services.AddScoped<UpdateUserService>();
            services.AddScoped<DeleteUserService>();

            // Person Services
            services.AddScoped<GetPersonsService>();
            services.AddScoped<GetPersonService>();
            services.AddScoped<CreatePersonService>();
            services.AddScoped<UpdatePersonService>();
            services.AddScoped<DeletePersonService>();

            // Roles Services
            services.AddScoped<GetRolesService>();
            services.AddScoped<GetRoleService>();
            services.AddScoped<CreateRoleService>();
            services.AddScoped<UpdateRoleService>();
            services.AddScoped<DeleteRoleService>();

            // Business Services
            services.AddScoped<GetBusinessesService>();
            services.AddScoped<GetBusinessService>();
            services.AddScoped<CreateBusinessService>();
            services.AddScoped<UpdateBusinessService>();
            services.AddScoped<DeleteBusinessService>();

            // Application Services
            services.AddScoped<GetApplicationsService>();
            services.AddScoped<GetApplicationService>();
            services.AddScoped<CreateApplicationService>();
            services.AddScoped<UpdateApplicationService>();
            services.AddScoped<DeleteApplicationService>();

            // Assign Roles Services
            services.AddScoped<AssignUserRoleService>();
            services.AddScoped<RemoveUserRoleService>();
            services.AddScoped<AssignApplicationRoleService>();
            services.AddScoped<RemoveApplicationRoleService>();
            services.AddScoped<GetUserRolesService>();
            services.AddScoped<GetApplicationRolesService>();

            return services;
        }
    }
}
