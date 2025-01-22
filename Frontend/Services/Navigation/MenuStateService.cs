using Artemis.Frontend.Models.Navigation;

namespace Artemis.Frontend.Services.Navigation
{
    public class MenuStateService
    {
        private List<ApplicationGroup> _menuGroups = [];
        private bool _isExpanded = true;

        public IReadOnlyList<ApplicationGroup> MenuGroups => _menuGroups.AsReadOnly();
        public bool IsExpanded => _isExpanded;
        public event Action? OnChange;

        public void UpdateMenuGroups(IEnumerable<ApplicationGroup> menuGroups)
        {
            _menuGroups = menuGroups.ToList();
            OnChange?.Invoke();
        }

        public void ToggleSidebar()
        {
            _isExpanded = !_isExpanded;
            OnChange?.Invoke();
        }

        public void SetMenuGroups(List<ApplicationGroup> groups)
        {
            _menuGroups = groups;
        }
    }
}
