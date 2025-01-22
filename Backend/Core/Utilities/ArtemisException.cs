namespace Artemis.Backend.Core.Utilities
{
    public class ArtemisException : Exception
    {
        public string DetailedMessage { get; }

        public ArtemisException(string message, string detailedMessage = "")
            : base($"{message} | Details: {detailedMessage}")
        {
            DetailedMessage = detailedMessage;
        }

        public ArtemisException(string message, Exception innerException)
            : base($"{message} | Details: {innerException.Message}", innerException)
        {
            DetailedMessage = innerException.Message;
        }

        public override string ToString()
        {
            return $"{Message} | Detailed Message: {DetailedMessage}";
        }
    }
}