namespace Artemis.Frontend.Services.Notification
{
    public enum NotificationType
    {
        Success,
        Error,
        Info,
        Warning
    }

    public class Notification
    {
        public string Message { get; set; } = string.Empty;
        public NotificationType Type { get; set; }
        public bool ShowCloseButton { get; set; } = true;
        public int DurationInSeconds { get; set; } = 3;
        public string Id { get; set; } = Guid.NewGuid().ToString();
    }

    public class NotificationService()
    {
        private readonly List<Notification> _notifications = [];

        public IReadOnlyList<Notification> Notifications => _notifications.AsReadOnly();
        public event Action? OnChange;

        public void Show(string message, NotificationType type = NotificationType.Info)
        {
            var notification = new Notification
            {
                Message = message,
                Type = type,
                Id = Guid.NewGuid().ToString()
            };

            _notifications.Add(notification);
            NotifyStateChanged();

            // Start a background task to remove the notification after the duration
            Task.Run(async () =>
            {
                await Task.Delay(TimeSpan.FromSeconds(notification.DurationInSeconds));
                Remove(notification.Id);
            });
        }

        public void Remove(string id)
        {
            var notification = _notifications.FirstOrDefault(n => n.Id == id);
            if (notification != null)
            {
                _notifications.Remove(notification);
                NotifyStateChanged();
            }
        }

        private void NotifyStateChanged()
        {
            OnChange?.Invoke();
        }

        public void ShowSuccess(string message) => Show(message, NotificationType.Success);
        public void ShowError(string message) => Show(message, NotificationType.Error);
        public void ShowInfo(string message) => Show(message, NotificationType.Info);
        public void ShowWarning(string message) => Show(message, NotificationType.Warning);
    }
}