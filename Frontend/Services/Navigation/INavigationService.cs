using Artemis.Frontend.Models.Navigation;

namespace Artemis.Frontend.Services.Navigation
{
    public interface INavigationService
    {
        Task<IEnumerable<ApplicationGroup>> GetUserMenuAsync();
    }
}
