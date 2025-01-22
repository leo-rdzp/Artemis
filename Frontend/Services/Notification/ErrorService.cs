namespace Artemis.Frontend.Services.Notification
{
    public class ErrorService
    {
        public string? LastError { get; private set; }
        public Exception? LastException { get; private set; }

        public void SetError(string message, Exception? exception = null)
        {
            LastError = message;
            LastException = exception;
        }

        public void ClearError()
        {
            LastError = null;
            LastException = null;
        }
    }
}
